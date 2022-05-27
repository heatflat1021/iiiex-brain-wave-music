using System.Collections.Generic;
using UnityEngine;
using EmotivUnityPlugin;
using Zenject;
using System.Linq;

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

        private readonly List<Channel_t> BandPowerTargetChannels = new List<Channel_t> {
            Channel_t.CHAN_AF3, Channel_t.CHAN_F7, Channel_t.CHAN_F3, Channel_t.CHAN_FC5, // 左前方
            Channel_t.CHAN_T7, Channel_t.CHAN_P7, Channel_t.CHAN_O1, // 左後方
            Channel_t.CHAN_AF4, Channel_t.CHAN_F8, Channel_t.CHAN_F4, Channel_t.CHAN_FC6, // 右前方
            Channel_t.CHAN_T8, Channel_t.CHAN_P8, Channel_t.CHAN_O2, // 右後方
        };

        private readonly List<CerebrumArea.CerebrumArea_t> TargetCerebrumAreas = new List<CerebrumArea.CerebrumArea_t> {
            CerebrumArea.CerebrumArea_t.LEFT_FRONTAL_LOBE,
            CerebrumArea.CerebrumArea_t.LEFT_OCCIPITAL_LOBE,
            CerebrumArea.CerebrumArea_t.RIGHT_FRONTAL_LOBE,
            CerebrumArea.CerebrumArea_t.RIGHT_OCCIPITAL_LOBE,
        };

        private GroupedSignals groupedSignals;

        private const float ThetaSoundThreshold = 1.7f;
        private const float AlphaSoundThreshold = 1.7f;
        private const float BetaLSoundThreshold = 1.7f;
        private const float BetaHSoundThreshold = 1.7f;
        private const float Delta = 0.0001f;

        public GameObject soundGameObject;
        private SoundManager soundManager;

        void Start()
        {
            groupedSignals = new GroupedSignals();

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
                groupedSignals.DeleteAll();

                foreach (var channel in BandPowerTargetChannels)
                {
                    double theta = DataStreamManager.Instance.GetThetaData(channel);
                    double alpha = DataStreamManager.Instance.GetAlphaData(channel);
                    double betaL = DataStreamManager.Instance.GetLowBetaData(channel);
                    double betaH = DataStreamManager.Instance.GetHighBetaData(channel);

                    CerebrumArea.CerebrumArea_t? cerebrumArea = CerebrumArea.ConvertChannelToCerebrumArea(channel);
                    if (cerebrumArea != null)
                    {
                        groupedSignals.Add(cerebrumArea.Value, Band.Band_t.THETA, theta);
                        groupedSignals.Add(cerebrumArea.Value, Band.Band_t.ALPHA, alpha);
                        groupedSignals.Add(cerebrumArea.Value, Band.Band_t.LOW_BETA, betaL);
                        groupedSignals.Add(cerebrumArea.Value, Band.Band_t.HIGH_BETA, betaH);
                    }
                }


                foreach (var cerebrumArea in TargetCerebrumAreas)
                {
                    // theta
                    double thetaPowerValue = groupedSignals.Get(cerebrumArea, Band.Band_t.THETA).Average();
                    if (thetaPowerValue > Delta)
                    {
                        if (thetaPowerValue > ThetaSoundThreshold)
                        {
                            soundManager.SoundStart(cerebrumArea, BandPowerType.Thetal);
                        }
                        else
                        {
                            soundManager.SoundStop(cerebrumArea, BandPowerType.Thetal);
                        }
                    }

                    // alpha
                    double alphaPowerValue = groupedSignals.Get(cerebrumArea, Band.Band_t.ALPHA).Average();
                    if (alphaPowerValue > Delta)
                    {
                        if (alphaPowerValue > AlphaSoundThreshold)
                        {
                            soundManager.SoundStart(cerebrumArea, BandPowerType.Alpha);
                        }
                        else
                        {
                            soundManager.SoundStop(cerebrumArea, BandPowerType.Alpha);
                        }
                    }

                    // low beta
                    double betaLPowerValue = groupedSignals.Get(cerebrumArea, Band.Band_t.LOW_BETA).Average();
                    if (betaLPowerValue > Delta)
                    {
                        if (betaLPowerValue > BetaLSoundThreshold)
                        {
                            soundManager.SoundStart(cerebrumArea, BandPowerType.BetalL);
                        }
                        else
                        {
                            soundManager.SoundStop(cerebrumArea, BandPowerType.BetalL);
                        }
                    }

                    double betaHPowerValue = groupedSignals.Get(cerebrumArea, Band.Band_t.HIGH_BETA).Average();
                    if (betaHPowerValue > Delta)
                    {
                        if (betaHPowerValue > BetaHSoundThreshold)
                        {
                            soundManager.SoundStart(cerebrumArea, BandPowerType.BetalH);
                        }
                        else
                        {
                            soundManager.SoundStop(cerebrumArea, BandPowerType.BetalH);
                        }
                    }

                    Debug.Log($"t:{thetaPowerValue}, a:{alphaPowerValue}, l:{betaLPowerValue}, h:{betaHPowerValue}");
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

        public void onStopBtnClick()
        {

        }
    }
}
