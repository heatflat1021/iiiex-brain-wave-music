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
        private DataStreamManager _dataStreamMgr = DataStreamManager.Instance;

        private float _timerDataUpdate = 0;
        private const float TIME_UPDATE_DATA = 0.3f;

        private readonly List<Channel_t> BandPowerTargetChannel = new List<Channel_t> {
            Channel_t.CHAN_AF3, Channel_t.CHAN_F7, Channel_t.CHAN_F3, Channel_t.CHAN_FC5, // 左前方
            Channel_t.CHAN_T7, Channel_t.CHAN_P7, Channel_t.CHAN_O1, // 左後方
            Channel_t.CHAN_AF4, Channel_t.CHAN_F8, Channel_t.CHAN_F4, Channel_t.CHAN_FC6, // 右前方
            Channel_t.CHAN_T8, Channel_t.CHAN_P8, Channel_t.CHAN_O2, // 右後方
        };

        private const float SoundThreshold = 0.01f;

        public GameObject soundGameObject;
        private SoundManager soundManager;

        void Start()
        {
            soundManager = soundGameObject.GetComponent<SoundManager>();
        }


        void Update()
        {
            if (!this.isActive)
            {
                return;
            }

            _timerDataUpdate += Time.deltaTime;
            if (_timerDataUpdate < TIME_UPDATE_DATA)
            {
                return;
            }
            _timerDataUpdate -= TIME_UPDATE_DATA;


            if (_dataStreamMgr.GetNumberPowerBandSamples() > 0)
            {
                List<Channel_t> list = DataStreamManager.Instance.GetEEGChannels();
                foreach (var channel in BandPowerTargetChannel)
                {
                    double theta = DataStreamManager.Instance.GetThetaData(channel);
                    double alpha = DataStreamManager.Instance.GetAlphaData(channel);
                    double betaL = DataStreamManager.Instance.GetLowBetaData(channel);
                    double betaH = DataStreamManager.Instance.GetHighBetaData(channel);

                    // todo: Once the specifications are finalized, rewrite them briefly.
                    if (theta > SoundThreshold)
                    {
                        soundManager.SoundStart(channel, BandPowerType.Thetal);
                    }
                    else
                    {
                        soundManager.SoundStop(channel, BandPowerType.Thetal);
                    }

                    if (alpha > SoundThreshold)
                    {
                        soundManager.SoundStart(channel, BandPowerType.Alpha);
                    }
                    else
                    {
                        soundManager.SoundStop(channel, BandPowerType.Alpha);
                    }

                    if (betaL > SoundThreshold)
                    {
                        soundManager.SoundStart(channel, BandPowerType.BetalL);
                    }
                    else
                    {
                        soundManager.SoundStop(channel, BandPowerType.BetalH);
                    }

                    if (betaH > SoundThreshold)
                    {
                        soundManager.SoundStart(channel, BandPowerType.BetalH);
                    }
                    else
                    {
                        soundManager.SoundStop(channel, BandPowerType.BetalH);
                    }

                    Debug.Log($"[{channel}] t:{theta}, a:{alpha}, l:{betaL}, h:{betaH}");
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
