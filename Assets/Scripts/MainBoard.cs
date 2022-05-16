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
                foreach (var channel in DataStreamManager.Instance.GetEEGChannels())
                {
                    Debug.Log(channel);
                    double theta = DataStreamManager.Instance.GetAlphaData(channel);
                    Debug.Log(theta);
                }
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
