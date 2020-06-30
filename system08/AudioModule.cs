using System;
using CSCore.CoreAudioAPI;

namespace system08
{
    delegate void EnumAudioSessionDelegation(AudioSessionControl sessionControl);

    class AudioModule
    {

        static void GetAudioSessions(EnumAudioSessionDelegation callback)
        {
            // Get default audio device and enumerators.
            MMDevice device = MMDeviceEnumerator.DefaultAudioEndpoint(CSCore.CoreAudioAPI.DataFlow.Render, CSCore.CoreAudioAPI.Role.Multimedia);
            AudioSessionManager2 manager = AudioSessionManager2.FromMMDevice(device);
            AudioSessionEnumerator enumerator = manager.GetSessionEnumerator();

            foreach (AudioSessionControl sessionControl in enumerator)
            {
                // Callback.
                callback(sessionControl);
            }
        }

        /// <summary>
        /// Apply volumes.
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="percentage"></param>
        /// <returns></returns>
        public static bool Apply(int processId, int percentage)
        {
            // Percentage validation.
            if (percentage < 0 || percentage > 100) return false;

            bool isApplied = false;

            // Apply percentage.
            GetAudioSessions((AudioSessionControl session) => {
                AudioSessionControl2 session2 = session.QueryInterface<AudioSessionControl2>();
                // If the process is equal.
                if (session2.ProcessID == processId)
                {
                    SimpleAudioVolume volume = session.QueryInterface<SimpleAudioVolume>();
                    volume.MasterVolume = (float)percentage / 100f;
                    isApplied = true;
                }
            });

            return isApplied;
        }
    }
}
