// C3 音量処理部で実装する関数の実装
// -----
// AL18004 秋山 久遠


using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CSCore.CoreAudioAPI;

namespace system08
{
    delegate void EnumAudioSessionDelegation(AudioSessionControl sessionControl);

    class AudioModule
    {
        /// <summary>
        /// CSCore を用いたオーディオセッションの取得
        /// </summary>
        /// <param name="callback">各セッションに対し行う関数</param>
        static void GetAudioSessions(EnumAudioSessionDelegation callback)
        {
            // デフォルトオーディオエンドポイントとイテレータの取得
            MMDevice device = MMDeviceEnumerator.DefaultAudioEndpoint(CSCore.CoreAudioAPI.DataFlow.Render, CSCore.CoreAudioAPI.Role.Multimedia);
            AudioSessionManager2 manager = AudioSessionManager2.FromMMDevice(device);
            AudioSessionEnumerator enumerator = manager.GetSessionEnumerator();

            foreach (AudioSessionControl sessionControl in enumerator)
            {
                // 関数呼び出し
                callback(sessionControl);
            }
        }

        /// <summary>
        /// アプリケーションの音量をdouble型で取得
        /// </summary>
        /// <param name="processId">プロセスID</param>
        /// <returns>
        /// 音量(0~1) as double.
        /// -1 means no audio.
        /// </returns>
        public static double GetVolume(int processId)
        {
            double volume = -1;
            Process process = Process.GetProcessById(processId);

            // Call from a new thread.
            Task.Run(() => GetAudioSessions((AudioSessionControl session) =>
            {
                try
                {
                    AudioSessionControl2 session2 = session.QueryInterface<AudioSessionControl2>();

                    // // Ideal is branch if the process id is equal. (not working)
                    // if (session2.ProcessID == processId)

                    // モジュールのパスが同じであるか
                    if (process.MainModule != null && session2.Process.MainModule.FileName == process.MainModule.FileName)
                    {
                        SimpleAudioVolume audioVolume = session.QueryInterface<SimpleAudioVolume>();
                        volume = audioVolume.MasterVolume;
                    }
                }
                catch (Exception e)
                {
                    // Usually Win32Exception.
                    Console.WriteLine(e.Message);
                }
            }));

            return volume;
        }

        /// <summary>
        /// アプリケーションの音量を設定
        /// </summary>
        /// <param name="processId">プロセスID</param>
        /// <param name="percentage">音量(0~1)</param>
        /// <returns>
        /// 音量が設定できたか
        /// </returns>
        public static bool SetVolume(int processId, double percentage)
        {
            // Percentage validation.
            if (percentage < 0 || percentage > 1) return false;

            bool isApplied = false;
            Process process = Process.GetProcessById(processId);

            // Call from a new thread.
            Task.Run(() => GetAudioSessions((AudioSessionControl session) => {
                try
                {
                    AudioSessionControl2 session2 = session.QueryInterface<AudioSessionControl2>();

                    // // Ideal is branch if the process id is equal. (not working)
                    // if (session2.ProcessID == processId)

                    // モジュールのパスが同じであるか
                    if (process.MainModule != null && session2.Process.MainModule.FileName == process.MainModule.FileName)
                    {
                        SimpleAudioVolume volume = session.QueryInterface<SimpleAudioVolume>();
                        volume.MasterVolume = (float)percentage;
                        isApplied = true;
                    }
                } catch(Exception e)
                {
                    // Win32Exceptionなど
                    Console.WriteLine(e.Message);
                }
            }));

            return isApplied;
        }
    }
}
