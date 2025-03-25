using BrowserPrint.Models;

using Neon.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserPrint
{
    public class BrowserPrintService
    {
        private BrowserPrintProvider BrowserPrintProvider;
        private TaskCompletionSource tcs;

        public BrowserPrintService()
        {
            tcs = new TaskCompletionSource();
        }

        public void RegisterComponent(BrowserPrintProvider BrowserPrintProvider)
        {
            this.BrowserPrintProvider = BrowserPrintProvider;

            if (tcs.Task.IsCompleted)
            {
                return;
            }

            tcs.TrySetResult();
        }

        public async Task<IEnumerable<Device>> GetAvailablePrintersAsync()
        {
            await SyncContext.Clear;

            await tcs.Task;

            return await BrowserPrintProvider.GetAvailablePrintersAsync();
        }

        public async Task<Device> GetDefaultPrinterAsync()
        {
            await SyncContext.Clear;

            await tcs.Task;

            return await BrowserPrintProvider.GetDefaultPrinterAsync();
        }

        public async Task PrintAsync(Device device, string data)
        {
            await SyncContext.Clear;

            await tcs.Task;

            await BrowserPrintProvider.PrintAsync(device, data);
        }

        public async Task SetPrinterAsync(Device device)
        {
            await SyncContext.Clear;

            await tcs.Task;

            await BrowserPrintProvider.SetPrinterAsync(device);
        }

        public async Task<PrinterStatus> CheckPrinterStatusAsync(Device device)
        {
            await SyncContext.Clear;

            await tcs.Task;

            return await BrowserPrintProvider.CheckPrinterStatusAsync(device);
        }
    }
}
