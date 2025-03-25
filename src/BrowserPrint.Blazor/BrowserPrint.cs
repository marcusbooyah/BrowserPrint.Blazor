using BrowserPrint.Models;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

using Neon.Blazor.Attributes;
using Neon.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrowserPrint
{
    [InteractiveAuto]
    public partial class BrowserPrintProvider : ComponentBase
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        public BrowserPrintService BrowserPrintService { get; set; }

        public Device Device { get; set; }

        private IJSObjectReference interop;

        private IEnumerable<Device> devices = Enumerable.Empty<Device>();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender && RendererInfo.IsInteractive)
            {
                await InitAsync();
            }
        }

        private async Task<bool> InitAsync()
        {
            if (!RendererInfo.IsInteractive)
            {
                return false;
            }

            interop ??= await JSRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/BrowserPrint.Blazor/js/interop.js");

            BrowserPrintService.RegisterComponent(this);

            return true;
        }

        public async Task<IEnumerable<Device>> GetAvailablePrintersAsync()
        {
            await SyncContext.Clear;

            if (!await InitAsync())
            {
                return Enumerable.Empty<Device>();
            }

            var devices = await interop.InvokeAsync<DeviceResult>("fetchJson", "http://localhost:9100/available");
            return devices.Printer;
        }

        public async Task<Device> GetDefaultPrinterAsync()
        {
            await SyncContext.Clear;

            if (!await InitAsync())
            {
                return null;
            }

            var device = await interop.InvokeAsync<string>("fetchText", "http://localhost:9100/default");

            devices = await GetAvailablePrintersAsync();

            var lines = device.Split('\n');
            var id = lines.Where(l => l.Contains("ID:")).First().Split(":", StringSplitOptions.RemoveEmptyEntries).Last().Trim();

            var defaultDevice = devices.Where(d => d.Uid == id).FirstOrDefault();

            return defaultDevice;
        }

        public async Task PrintAsync(Device device, string data)
        {
            await SyncContext.Clear;

            if (!await InitAsync())
            {
                return;
            }

            var payload = new PrintPayload()
            {
                Device = device,
                Data = data
            };

            await interop.InvokeVoidAsync("postJson", "http://localhost:9100/write", payload);
        }

        public async Task SetPrinterAsync(Device device)
        {
            await SyncContext.Clear;

            this.Device = device;
        }

        public async Task<Device> GetPrinterAsync()
        {
            await SyncContext.Clear;

            return Device;
        }

        public async Task<PrinterStatus> CheckPrinterStatusAsync(Device device = null)
        {
            await SyncContext.Clear;

            if (!await InitAsync())
            {
                return null;
            }

            device ??= Device;

            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            var payload = new PrintPayload()
            {
                Device = device,
                Data = "~HQES"
            };

            await interop.InvokeVoidAsync("postJson", "http://localhost:9100/write", payload);

            payload.Data = null;

            var status = await interop.InvokeAsync<string>("postText", "http://localhost:9100/read", payload);

            bool isReady = false;
            List<string> errors = [];

            var error = int.Parse(status.Substring(70, 1));
            var media = int.Parse(status.Substring(88, 1));
            var head = int.Parse(status.Substring(87, 1));
            var pause = int.Parse(status.Substring(84, 1));

            isReady = error == 0;
            
            if (pause == 1)
            {
                errors.Add("Printer is paused");
            }

            switch (media)
            {
                case 1:
                    errors.Add("Paper out");
                    break;
                case 2:
                    errors.Add("Ribbon out");
                    break;
                case 4:
                    errors.Add("Media Door Open");
                    break;
                case 8:
                    errors.Add("Cutter Fault");
                    break;

                default:
                    break;
            }

            switch (head)
            {
                case 1:
                    errors.Add("Printhead Overheating");
                    break;
                case 2:
                    errors.Add("Motor Overheating");
                    break;
                case 4:
                    errors.Add("Printhead Fault");
                    break;
                case 8:
                    errors.Add("Incorrect Printhead");
                    break;
                default:
                    break;
            }

            if (!isReady && errors.Count == 0) errors.Add("Error: Unknown Error");

            return new PrinterStatus()
            {
                IsReady = isReady,
                Errors = errors
            };
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "id", "browserPrintProvider");
            builder.CloseElement();
        }
    }
}
