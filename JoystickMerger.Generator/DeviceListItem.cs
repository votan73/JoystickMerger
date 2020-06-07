using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.DirectInput;

namespace JoystickMerger.Generator
{
    partial class DeviceListItem : UserControl
    {
        public DeviceListItem()
        {
            InitializeComponent();
            DeadZone = 8.4f;
        }
        protected override void OnLoad(EventArgs e)
        {
            if (!DesignMode)
            {
                base.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
                Text = Item.Name;
                input = input ?? new DirectInput();

                var dev = new Joystick(input, Item.Device.InstanceGuid);
                try
                {
                    dev.SetCooperativeLevel(FindForm().Handle, CooperativeLevel.Background | CooperativeLevel.NonExclusive);
                    dev.Acquire();
                    Instance = dev;
                    var caps = dev.Capabilities;

                    ButtonCount = caps.ButtonCount;
                    POVCount = caps.PovCount;

                    Task.Factory.StartNew(() => { Instance.Poll(); state = Instance.GetCurrentState(); });
                    if (pollTimer == null)
                    {
                        var names = new HashSet<string>(DataSources.JoystickAxisTemplate.Select<JoyKeyValue, string>(x => x.Key));
                        props = typeof(JoystickState).GetProperties().Where<System.Reflection.PropertyInfo>(x => names.Contains(x.Name)).ToArray<System.Reflection.PropertyInfo>();

                        pollTimer = new System.Threading.Timer((state) => { if (OnTimer != null) OnTimer(pollTimer, EventArgs.Empty); }, null, 100, 100);
                    }
                    OnTimer += DeviceListItem_OnTimer;
                }
                catch(System.Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    dev.Dispose();
                }
            }
            base.OnLoad(e);
        }

        private bool SignificatChange(int current, int state)
        {
            return (Math.Abs(current - state) / 32768.0f) >= 0.3333;
        }

        void DeviceListItem_OnTimer(object sender, EventArgs e)
        {
            if (state == null)
                return;

            Instance.Poll();
            var current = Instance.GetCurrentState();
            for (int i = 0; i < ButtonCount; i++)
            {
                if (current.Buttons[i] != state.Buttons[i])
                {
                    state.Buttons[i] = current.Buttons[i];
                    DetectedType = DetectionType.Button;
                    DetectedValue = (i + 1).ToString();
                    LastChangeDectection = DateTime.Now;
                    return;
                }
            }
            for (int i = 0; i < POVCount; i++)
            {
                if (current.PointOfViewControllers[i] != state.PointOfViewControllers[i])
                {
                    state.PointOfViewControllers[i] = current.PointOfViewControllers[i];
                    DetectedType = DetectionType.PointOfView;
                    DetectedValue = (i + 1).ToString();
                    LastChangeDectection = DateTime.Now;
                    return;
                }
            }
            foreach (var prop in props)
            {
                if (SignificatChange((int)prop.GetValue(current), (int)prop.GetValue(state)))
                {
                    prop.SetValue(state, prop.GetValue(current));
                    DetectedType = DetectionType.Axis;
                    DetectedValue = prop.Name;
                    LastChangeDectection = DateTime.Now;
                    return;
                }
            }
            for (int i = 0; i < current.Sliders.Length; i++)
            {
                if (SignificatChange(current.Sliders[i], state.Sliders[i]))
                {
                    state.Sliders[i] = current.Sliders[i];
                    DetectedType = DetectionType.Slider;
                    DetectedValue = (i + 1).ToString();
                    LastChangeDectection = DateTime.Now;
                    return;
                }
            }
        }

        protected override void DestroyHandle()
        {
            OnTimer -= DeviceListItem_OnTimer;
            if (Instance != null)
                Instance.Dispose();
            base.DestroyHandle();
        }

        public override string Text { get { return checkBox1.Text; } set { checkBox1.Text = value; } }

        static DirectInput input;
        public DeviceItem Item;
        public Joystick Instance { get; private set; }
        private JoystickState state;
        static System.Reflection.PropertyInfo[] props;
        private int ButtonCount;
        private int POVCount;

        static System.Threading.Timer pollTimer;
        static event EventHandler OnTimer;

        public override string ToString()
        {
            return String.Concat(Item.Name, " (", Item.Device.InstanceGuid, ")");
        }

        private void SliderDeadzone_ValueChanged(object sender, EventArgs e)
        {
            label2.Text = (SliderDeadzone.Value / 1000f).ToString("p01");
        }

        public float DeadZone { get { return SliderDeadzone.Value / 10f; } set { SliderDeadzone.Value = Math.Min(500, Math.Max(0, Convert.ToInt32(value * 10f))); } }

        public DateTime LastChangeDectection;
        public DetectionType DetectedType;
        public string DetectedValue;

    }
}
