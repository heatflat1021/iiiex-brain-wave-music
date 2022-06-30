using System.Collections.Generic;
using UnityEngine;
using EmotivUnityPlugin;
using Zenject;
using System.Linq;
using UnityEngine.UI;

namespace dirox.emotiv.controller
{
    /// <summary>
    /// Board of examples
    /// </summary>
    public class MainBoard : BaseCanvasView
    {
        public GameObject eegButton;

        public GameObject centerCircle;

        public GameObject electrode;
        public GameObject electrodeOrange;
        public GameObject electrodeSkyBlue;
        public GameObject electrodeShadow;
        public GameObject circleEffect;
        public GameObject pentagonEffect;
        public GameObject squareEffect;
        public GameObject triangleEffect;

        private DataStreamManager _dataStreamMgr = DataStreamManager.Instance;

        private float _timerDataUpdate = 0;
        private const float TIME_UPDATE_DATA = 0.1f;
        private const double CONTACT_QUALITY_THRESHOLD = 0.3;

        private readonly List<Channel_t> BandPowerTargetChannels = new List<Channel_t> {
            Channel_t.CHAN_AF3, Channel_t.CHAN_F7, Channel_t.CHAN_F3, Channel_t.CHAN_FC5,
            Channel_t.CHAN_T7, Channel_t.CHAN_P7, Channel_t.CHAN_O1,
            Channel_t.CHAN_AF4, Channel_t.CHAN_F8, Channel_t.CHAN_F4, Channel_t.CHAN_FC6,
            Channel_t.CHAN_T8, Channel_t.CHAN_P8, Channel_t.CHAN_O2,
        };

        private readonly List<CerebrumArea.CerebrumArea_t> TargetCerebrumAreas = new List<CerebrumArea.CerebrumArea_t> {
            CerebrumArea.CerebrumArea_t.LEFT_OUTER,
            CerebrumArea.CerebrumArea_t.LEFT_MIDDLE,
            CerebrumArea.CerebrumArea_t.LEFT_CENTER,
            CerebrumArea.CerebrumArea_t.RIGHT_CENTER,
            CerebrumArea.CerebrumArea_t.RIGHT_MIDDLE,
            CerebrumArea.CerebrumArea_t.RIGHT_OUTER,
        };

        private GroupedSignals groupedSignals;
        private ContactQualityHistory contactQualityHistory;

        private const float ThetaSoundThreshold = 1.7f;
        private const float AlphaSoundThreshold = 1.7f;
        private const float BetaLSoundThreshold = 1.7f;
        private const float BetaHSoundThreshold = 1.7f;
        private const float Delta = 0.0001f;

        public GameObject soundGameObject;
        private SoundManager soundManager;

        private GameObject electrodeComponents;
        private GameObject waveComponents;
        private GameObject centerComponents;

        private static readonly Color ColorOrange = new Color(255, 109, 0, 1.0f);
        private static readonly Color ColorOrangeLight = new Color(255, 109, 0, 0.2f);
        private static readonly Color ColorSkyBlue = new Color(0, 183, 206, 0.4f);

        private static readonly int ElectrodeDistanceX = 50;
        private static readonly int ElectrodeDistanceY = 50;
        private static readonly int ElectrodeShadowGapX = 1;
        private static readonly int ElectrodeShadowGapY = 1;

        private bool isInitialized = false;
        private const float CircleGenerationThreshold = 1.7f;
        private const float CircleScaleMaxMagnitude = 8.0f;
        private const float CircleScaleIncreasingRate = 1.08f;
        private const float CircleOpacityDecreasingRate = 0.92f;

        void Start()
        {
            groupedSignals = new GroupedSignals();
            contactQualityHistory = new ContactQualityHistory();

            soundManager = soundGameObject.GetComponent<SoundManager>();

            electrodeComponents = transform.Find("ViewComponents/ElectrodeComponents").gameObject;
            waveComponents = transform.Find("ViewComponents/WaveComponents").gameObject;
            centerComponents = transform.Find("ViewComponents/CenterComponents").gameObject;
        }


        void Update()
        {
            if (!this.isActive)
            {
                return;
            }

            if (!isInitialized)
            {
                for (int x=-10; x<=10; x++)
                {
                    for (int y=-6; y<=6; y++)
                    {
                        if ((x == 10 && y == 6) || (x == 9 && y == 6) || (x == 0) || ((-3 <= x && x <= 3) && (-3 <= y && y <= 3) && !(x == -3 && y == -3) && !(x == -3 && y == 3) && !(x == 3 && y == -3) && !(x == 3 && y == 3)))
                            continue;

                        GameObject electrodeShadowObject = (GameObject)Instantiate(electrodeShadow, electrodeComponents.transform);
                        electrodeShadowObject.transform.localPosition = new Vector3(x * (ElectrodeDistanceX + ElectrodeShadowGapX), y * (ElectrodeDistanceY + ElectrodeShadowGapY), 0);
                        electrodeShadowObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                        electrodeShadowObject.GetComponent<Image>().color = (x < 0) ? new Color(0, 183, 206, 0.2f) : new Color(253, 126, 0, 0.2f);

                        if ((x==-7 && y==4) || (x==-4 && y==5) || (x==-7 && y==2) || (x==-4 && y==2) || (x==-9 && y==-1) || (x==-6 && y==-3) || (x==-3 && y==-5))
                        {
                            GameObject electrodeObject = (GameObject)Instantiate(electrodeSkyBlue, electrodeComponents.transform);
                            electrodeObject.transform.localPosition = new Vector3(x * ElectrodeDistanceX, y * ElectrodeDistanceY, 0);
                            electrodeObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                            electrodeObject.GetComponent<Image>().color = new Color(30, 30, 30);
                        }
                        else if ((x == 7 && y == 4) || (x == 4 && y == 5) || (x == 7 && y == 2) || (x == 4 && y == 2) || (x == 9 && y == -1) || (x == 6 && y == -3) || (x == 3 && y == -5))
                        {
                            GameObject electrodeObject = (GameObject)Instantiate(electrodeOrange, electrodeComponents.transform);
                            electrodeObject.transform.localPosition = new Vector3(x * ElectrodeDistanceX, y * ElectrodeDistanceY, 0);
                            electrodeObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                            electrodeObject.GetComponent<Image>().color = new Color(30, 30, 30);
                        }
                        else
                        {
                            GameObject electrodeObject = (GameObject)Instantiate(electrode, electrodeComponents.transform);
                            electrodeObject.transform.localPosition = new Vector3(x * ElectrodeDistanceX, y * ElectrodeDistanceY, 0);
                            electrodeObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                            electrodeObject.GetComponent<Image>().color = new Color(30, 30, 30);
                        }
                    }
                }

                isInitialized = true;
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

                    double contactQuality = DataStreamManager.Instance.GetContactQuality(channel);
                    contactQualityHistory.Add(channel, contactQuality);
                    if (contactQualityHistory.Get(channel).Max() < CONTACT_QUALITY_THRESHOLD)
                    {
                        continue;
                    }

                    CerebrumArea.CerebrumArea_t? cerebrumArea = CerebrumArea.ConvertChannelToCerebrumArea(channel);
                    if (cerebrumArea != null)
                    {
                        groupedSignals.Add(cerebrumArea.Value, Band.Band_t.THETA, theta);
                        groupedSignals.Add(cerebrumArea.Value, Band.Band_t.ALPHA, alpha);
                        groupedSignals.Add(cerebrumArea.Value, Band.Band_t.LOW_BETA, betaL);
                        groupedSignals.Add(cerebrumArea.Value, Band.Band_t.HIGH_BETA, betaH);
                    }

                    if (theta > CircleGenerationThreshold)
                    {
                        GameObject circleObject = (GameObject)Instantiate(circleEffect, waveComponents.transform);
                        circleObject.transform.localPosition = GetCirclePosition(channel);
                        circleObject.transform.GetComponent<Image>().color = ColorSkyBlue;
                    }
                    else if (alpha > CircleGenerationThreshold)
                    {
                        GameObject pentagonObject = (GameObject)Instantiate(pentagonEffect, waveComponents.transform);
                        pentagonObject.transform.localPosition = GetCirclePosition(channel);
                        pentagonObject.transform.GetComponent<Image>().color = ColorSkyBlue;
                    }
                    else if (betaL > CircleGenerationThreshold)
                    {
                        GameObject squareObject = (GameObject)Instantiate(squareEffect, waveComponents.transform);
                        squareObject.transform.localPosition = GetCirclePosition(channel);
                        squareObject.transform.GetComponent<Image>().color = ColorOrangeLight;
                    }
                    else if (betaH > CircleGenerationThreshold)
                    {
                        GameObject triangleObject = (GameObject)Instantiate(triangleEffect, waveComponents.transform);
                        triangleObject.transform.localPosition = GetCirclePosition(channel);
                        triangleObject.transform.GetComponent<Image>().color = ColorOrangeLight;
                    }
                }

                foreach (var cerebrumArea in TargetCerebrumAreas)
                {
                    // theta
                    double thetaPowerValue = groupedSignals.Get(cerebrumArea, Band.Band_t.THETA).Count == 0 ? 0.0 : groupedSignals.Get(cerebrumArea, Band.Band_t.THETA).Average();
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
                    double alphaPowerValue = groupedSignals.Get(cerebrumArea, Band.Band_t.ALPHA).Count == 0 ? 0.0 : groupedSignals.Get(cerebrumArea, Band.Band_t.ALPHA).Average();
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
                    double betaLPowerValue = groupedSignals.Get(cerebrumArea, Band.Band_t.LOW_BETA).Count == 0 ? 0.0 : groupedSignals.Get(cerebrumArea, Band.Band_t.LOW_BETA).Average();
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

                    // high beta
                    double betaHPowerValue = groupedSignals.Get(cerebrumArea, Band.Band_t.HIGH_BETA).Count == 0 ? 0.0 : groupedSignals.Get(cerebrumArea, Band.Band_t.HIGH_BETA).Average();
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
                }
            }

            var waves = new Transform[waveComponents.transform.childCount];
            for (int i=0; i<waves.Length; i++)
            {
                waves[i] = waveComponents.transform.GetChild(i);
            }

            foreach(Transform wave in waves)
            {
                Vector3 scale = wave.localScale;

                if (scale.magnitude > CircleScaleMaxMagnitude)
                {
                    Destroy(wave.gameObject);
                }
                else
                {
                    wave.localScale = scale * CircleScaleIncreasingRate;

                    Color color = wave.GetComponent<Image>().color;
                    wave.GetComponent<Image>().color = new Color(color.r, color.g, color.b, color.a * CircleOpacityDecreasingRate);
                }
            }

            GameObject centerCircle = centerComponents.transform.Find("CenterCircle(Clone)").gameObject;
            centerCircle.transform.Rotate(new Vector3(0, 0, -5));
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

            eegButton.SetActive(false);

            GameObject centerCircleObject = (GameObject)Instantiate(centerCircle, centerComponents.transform);
            centerCircleObject.transform.localPosition = new Vector3(0, -40, 0);
            centerCircleObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            centerCircleObject.transform.GetComponent<Image>().color = new Color(255, 109, 0, 1.0f);
        }

        public void onStopBtnClick()
        {

        }

        private Vector3 GetCirclePosition(Channel_t channel)
        {
            int x=0, y=0;
            switch (channel)
            {
                case Channel_t.CHAN_AF3:
                    x = -7;
                    y = 4;
                    break;
                case Channel_t.CHAN_F7:
                    x = -4;
                    y = 5;
                    break;
                case Channel_t.CHAN_F3:
                    x = -7;
                    y = 2;
                    break;
                case Channel_t.CHAN_FC5:
                    x = -4;
                    y = 2;
                    break;
                case Channel_t.CHAN_T7:
                    x = -9;
                    y = -1;
                    break;
                case Channel_t.CHAN_P7:
                    x = -6;
                    y = -3;
                    break;
                case Channel_t.CHAN_O1:
                    x = -3;
                    y = -5;
                    break;
                case Channel_t.CHAN_AF4:
                    x = 7;
                    y = 4;
                    break;
                case Channel_t.CHAN_F8:
                    x = 4;
                    y = 5;
                    break;
                case Channel_t.CHAN_F4:
                    x = 7;
                    y = 2;
                    break;
                case Channel_t.CHAN_FC6:
                    x = 4;
                    y = 2;
                    break;
                case Channel_t.CHAN_T8:
                    x = 9;
                    y = -1;
                    break;
                case Channel_t.CHAN_P8:
                    x = 6;
                    y = -3;
                    break;
                case Channel_t.CHAN_O2:
                    x = 3;
                    y = -5;
                    break;
            }
            return new Vector3(x * ElectrodeDistanceX, y * ElectrodeDistanceY, -1);
        }
    }
}
