using Solution.Device.OpcUaDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.CommunicationService.Services
{
    public class SubScripttionDoWorkModel : OSharp.Utility.Disposable
    {
        public OpcUaDeviceHelper Device { get; set; }


        public OpcUaDeviceOutParamEntity OutEntity { get; set; }


        protected override void Disposing()
        {
            OutEntity.Dispose();
        }
    }
}
