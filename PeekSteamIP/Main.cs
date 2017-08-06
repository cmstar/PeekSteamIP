using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace PeekSteamIP
{
    public partial class Main : Form
    {
        private const int WorkerThreads = Env.ConnectionLimit;

        // ip gridview defination
        private const int ColIdxIp = 0;
        private const int ColIdxLocation = 1;
        private const int ColIdxElapsed = 2;
        private const int ColIdxStatus = 3;
        private const int ColIdxStatuCode = 4;
        private readonly int[] _ipGridColumns =
        {
            ColIdxIp, ColIdxLocation, ColIdxElapsed, ColIdxStatus, ColIdxStatuCode
        };

        // dns gridview defination
        private const int ColIdxDnsServer = 0;
        private const int ColIdxDnsName = 1;
        private const int ColIdxDnsResultIp = 2;
        private const int ColIdxDnsResultStatus = 3;
        private readonly int[] _dnsGridColumns =
        {
            ColIdxDnsServer, ColIdxDnsName, ColIdxDnsResultIp, ColIdxDnsResultStatus
        };

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;

            var ipNodes = IpNodeProvider.All();
            var ipColLen = _ipGridColumns.Length;
            foreach (var node in ipNodes)
            {
                var row = new object[ipColLen];
                row[ColIdxIp] = node.Ip;
                row[ColIdxLocation] = node.Location;
                gridIpList.Rows.Add(row);
            }

            var dnsNodes = DnsNodeProvider.All();
            var dnsColLen = _dnsGridColumns.Length;
            foreach (var node in dnsNodes)
            {
                var row = new object[dnsColLen];
                row[ColIdxDnsServer] = node.Server;
                row[ColIdxDnsName] = node.Name;
                gridDnsList.Rows.Add(row);
            }
        }

        private void btnBeginTest_Click(object sender, EventArgs e)
        {
            ResetTestingStatus();

            var ipTestThreads = PerformIpTest();
            PerformDnsTest(ipTestThreads);
        }

        private void gridIpList_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index != ColIdxElapsed)
                return;

            e.SortResult = CompareElapsedTime((string)e.CellValue1, (string)e.CellValue2);
            e.Handled = true;
        }

        private int CompareElapsedTime(string x, string y)
        {
            // 以字符串形式比较消耗的时间
            // 1 长度越大说明尾数越多，耗时越长
            // 2 长度一样，直接做字符串字典比较即可
            if (x == null && y == null)
                return 0;

            if (x == null)
                return 1;

            if (y == null)
                return -1;

            if (x.Length == y.Length)
                return x.CompareTo(y);

            return x.Length.CompareTo(y.Length);
        }

        private void PerformDnsTest(List<Thread> ipTestThreads)
        {
            const string msgFail = "解析失败";

            var thread = new Thread(_ =>
            {
                var len = gridDnsList.Rows.Count;

                for (int i = 0; i < len; i++)
                {
                    var row = gridDnsList.Rows[i];
                    var server = (string)row.Cells[ColIdxDnsServer].Value;
                    var result = DnsTester.ResolveSteamIp(server);

                    UpdateUI(() =>
                    {
                        row.Cells[ColIdxDnsResultIp].Value = result ?? msgFail;
                    });
                }

                ipTestThreads.ForEach(x => x.Join());

                var ipStatus = gridIpList.Rows
                    .Cast<DataGridViewRow>()
                    .ToDictionary(
                        row => (string)row.Cells[ColIdxIp].Value,
                        row => (string)row.Cells[ColIdxStatus].Value);

                for (int i = 0; i < len; i++)
                {
                    var row = gridDnsList.Rows[i];
                    var result = (string)row.Cells[ColIdxDnsResultIp].Value;

                    string status;
                    if (result == msgFail)
                    {
                        status = string.Empty;
                    }
                    else if (!ipStatus.TryGetValue(result, out status))
                    {
                        status = "未知IP";
                    }

                    UpdateUI(() =>
                    {
                        row.Cells[ColIdxDnsResultStatus].Value = status;
                    });
                }
            });

            thread.IsBackground = true;
            thread.Start();
        }

        private List<Thread> PerformIpTest()
        {
            var oldTest = btnBeginTest.Text;
            UpdateUI(() =>
            {
                btnBeginTest.Text = "测试中...";
                btnBeginTest.Enabled = false;
            });

            var total = gridIpList.Rows.Count;
            var finished = 0;
            var currentIndex = -1;
            var threads = new List<Thread>(WorkerThreads);

            for (int i = 0; i < WorkerThreads; i++)
            {
                var thread = new Thread(_ =>
                {
                    int idx;
                    while ((idx = Interlocked.Increment(ref currentIndex)) < total)
                    {
                        try
                        {
                            TestOne(idx);
                        }
                        finally
                        {
                            if (Interlocked.Increment(ref finished) == total)
                            {
                                UpdateUI(() =>
                                {
                                    btnBeginTest.Text = oldTest;
                                    btnBeginTest.Enabled = true;
                                });
                            }
                        }
                    }
                });

                thread.IsBackground = true;
                thread.Start();

                threads.Add(thread);
            }

            return threads;
        }

        private void ResetTestingStatus()
        {
            const string proccessing = "<->";

            foreach (DataGridViewRow row in gridIpList.Rows)
            {
                row.Cells[ColIdxElapsed].Value = string.Empty;
                row.Cells[ColIdxStatus].Value = proccessing;
                row.Cells[ColIdxStatuCode].Value = string.Empty;
            }

            foreach (DataGridViewRow row in gridDnsList.Rows)
            {
                row.Cells[ColIdxDnsResultIp].Value = proccessing;
                row.Cells[ColIdxDnsResultStatus].Value = proccessing;
            }
        }

        private void TestOne(int rowIndex)
        {
            var row = gridIpList.Rows[rowIndex];
            var ip = (string)row.Cells[ColIdxIp].Value;
            var testResult = IpTester.TestSteamIp(ip);
            var description = GetStatusDescription(testResult.Status);

            UpdateUI(() =>
            {
                // 耗时
                row.Cells[ColIdxElapsed].Value = testResult.ElapsedMilliseconds.ToString();

                // 状态
                row.Cells[ColIdxStatus].Value = description;

                // 状态码
                if (testResult.HttpStatusCode.HasValue)
                {
                    row.Cells[ColIdxStatuCode].Value = ((int)testResult.HttpStatusCode.Value).ToString();
                }
            });
        }

        private void UpdateUI(Action act)
        {
            if (InvokeRequired)
            {
                Invoke(act);
            }
            else
            {
                act();
            }
        }

        private string GetStatusDescription(IpStatus status)
        {
            switch (status)
            {
                case IpStatus.Good:
                    return "正常";

                case IpStatus.Timeout:
                    return "超时";

                case IpStatus.ContentError:
                    return "网页内容错误";

                case IpStatus.ConnectionReset:
                    return "连接被重置";

                case IpStatus.Exception:
                    return "其他异常";

                default:
                    return "未知";
            }
        }
    }
}
