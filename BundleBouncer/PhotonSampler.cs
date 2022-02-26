/**
* BundleBouncer Photon Sampler (Used for development)
* 
* Copyright (c) 2022 BundleBouncer Contributors
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

#if USING_PHOTON_SAMPLER
using System.Collections.Generic;
using System.IO;
using ExitGames.Client.Photon;
using Newtonsoft.Json;

namespace BundleBouncer
{
    internal class PhotonSampler
    {
        const int MAX_SAMPLES = 255;
        private const string Path = "UserData/BundleBouncer/photon.samples.json";
        public static Dictionary<byte, List<dynamic>> samples = new Dictionary<byte, List<dynamic>>();
        internal static void Sample(EventData ev)
        {
            if (ev is null)
            {
                return;
            }

            var key = ev.Code;
            var data = Patches.params2dynamic(ev.Parameters);
            if (!samples.ContainsKey(key))
            {
                samples[key] = new List<dynamic>(255);
            }
            if (samples[key].Count < 255 && !samples[key].Contains(data))
            {
                samples[key].Add(data);
                File.WriteAllText(Path, JsonConvert.SerializeObject(samples));
            }
        }
    }
}
#endif