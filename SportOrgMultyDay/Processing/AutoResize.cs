using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SportOrgMultyDay.Processing
{
    public class AutoResize
    {
        Form form;
        List<ControlSize> controls = new();
        public AutoResize(Form form)
        {
            this.form = form;
        }
        public void Add(Control control,bool weight = true,bool height = true)
        {
            int w = int.MinValue;
            int h = int.MinValue;
            if (weight)
                w = form.Width - control.Width;
            if (height)
                h = form.Height - control.Height;
            ControlSize controlSize = new(control, w, h);
            controls.Add(controlSize);
        }
        public void Update()
        {
            foreach (ControlSize cs in controls)
            {
                cs.Resize(form);
            }

            //Size tabControl1Size = TabControl.Size;
            //tabControl1Size.Width = form.Size.Width - weightRaznTC;
            //tabControl1Size.Height = form.Size.Height - heightRaznTC;
            //TabControl.Size = tabControl1Size;

            //Control q = TabControl;
            //Control w = richTextBox;
        }
    }
    class ControlSize
    {
        public Control control { get; set; }
        public int weight { get; set; }
        public int height { get; set; }
        public ControlSize(Control _control, int _weight, int _height)
        {
            weight = _weight;
            height = _height;
            control = _control;
        }
        public void Resize(Form form)
        {
            if (weight != int.MinValue)
                control.Width = form.Width - weight;
            if (height != int.MinValue)
                control.Height = form.Height - height;
        }
    }
}
