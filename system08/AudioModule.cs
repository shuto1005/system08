using System;
using System.Diagnostics;
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
        /// Get volumes as integer.
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="percentage"></param>
        /// <returns></returns>
        public static int GetVolume(int processId)
        {
            int volume = 0;
            Process process = Process.GetProcessById(processId);

            // Get by enumerator.
            GetAudioSessions((AudioSessionControl session) => {
                AudioSessionControl2 session2 = session.QueryInterface<AudioSessionControl2>();

                // // Ideal is branch if the process id is equal. (not working)
                // if (session2.ProcessID == processId)

                // If filename is the same.
                if (session2.Process.MainModule.FileName == process.MainModule.FileName)
                {
                    SimpleAudioVolume audioVolume = session.QueryInterface<SimpleAudioVolume>();
                    volume = (int)(audioVolume.MasterVolume * 100);
                }
            });

            return volume;
        }

        /// <summary>
        /// Set volumes.
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="percentage"></param>
        /// <returns></returns>
        public static bool SetVolume(int processId, int percentage)
        {
            // Percentage validation.
            if (percentage < 0 || percentage > 100) return false;

            bool isApplied = false;
            Process process = Process.GetProcessById(processId);

            // Apply percentage.
            GetAudioSessions((AudioSessionControl session) => {
                AudioSessionControl2 session2 = session.QueryInterface<AudioSessionControl2>();
                // // Ideal is branch if the process id is equal. (not working)
                // if (session2.ProcessID == processId)

                // If filename is the same.
                if (session2.Process.MainModule.FileName == process.MainModule.FileName)
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
