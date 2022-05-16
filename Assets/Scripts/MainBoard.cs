using System.Collections.Generic;
using UnityEngine;
using EmotivUnityPlugin;
using Zenject;

namespace dirox.emotiv.controller
{
    /// <summary>
    /// Board of examples
    /// </summary>
    public class MainBoard : BaseCanvasView
    {
        DataStreamManager _dataStreamMgr = DataStreamManager.Instance;

        float _timerDataUpdate = 0;
        const float TIME_UPDATE_DATA = 1f;

        void Update()
        {
            if (!this.isActive)
            {
                return;
            }

            _timerDataUpdate += Time.deltaTime;
            if (_timerDataUpdate < TIME_UPDATE_DATA)
                return;

            _timerDataUpdate -= TIME_UPDATE_DATA;

            Debug.Log(_dataStreamMgr.GetEEGChannels().Count);
            // update power band

            Debug.Log(_dataStreamMgr.GetBandPowerLists().ToString());
            Debug.Log(_dataStreamMgr.GetBandPowerLists().Count);

            if (_dataStreamMgr.GetNumberEEGSamples() > 0)
            {
                string eegHeaderStr = "EEG Header: ";
                string eegDataStr = "EEG Data: ";
                foreach (var channel in DataStreamManager.Instance.GetEEGChannels())
                {
                    Debug.Log(channel);
                    double theta = DataStreamManager.Instance.GetAlphaData(channel);
                    Debug.Log(theta);
                }
/*                foreach (var ele in DataStreamManager.Instance.GetEEGChannels())
                {
                    string chanStr = ChannelStringList.ChannelToString(ele);
                    double[] data = DataStreamManager.Instance.GetEEGData(ele);
                    eegHeaderStr += chanStr + ", ";
                    if (data != null && data.Length > 0)
                        eegDataStr += data[0].ToString() + ", ";
                    else
                        eegDataStr += "null, "; // for null value
                    Debug.Log(eegDataStr);
                }*/
            }
        }

        public override void Activate()
        {
            Debug.Log("MainBoard: Activate");
            base.Activate();
        }

        public override void Deactivate()
        {
            Debug.Log("MainBoard: Deactivate");
            base.Deactivate();
        }

        public void onStartBtnClick()
        {
            List<string> dataStreamList = new List<string>() { DataStreamName.EEG, DataStreamName.BandPower };
            _dataStreamMgr.SubscribeMoreData(dataStreamList);
        }
    }
}
