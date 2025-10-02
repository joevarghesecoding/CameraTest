using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CameraTest
{
    partial class CameraView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.camera1_groupbox = new System.Windows.Forms.GroupBox();
            this.focus_val_label = new System.Windows.Forms.Label();
            this.zoom1_val_label = new System.Windows.Forms.Label();
            this.brightness1_val_label = new System.Windows.Forms.Label();
            this.focus1_val_label = new System.Windows.Forms.Label();
            this.zoom1_label = new System.Windows.Forms.Label();
            this.zoom1_trackbar = new System.Windows.Forms.TrackBar();
            this.brightness1_label = new System.Windows.Forms.Label();
            this.focus1_label = new System.Windows.Forms.Label();
            this.brightness1_trackbar = new System.Windows.Forms.TrackBar();
            this.focus1_trackbar = new System.Windows.Forms.TrackBar();
            this.camera1_picturebox = new System.Windows.Forms.PictureBox();
            this.unitinfo_groupbox = new System.Windows.Forms.GroupBox();
            this.model_picturebox = new System.Windows.Forms.PictureBox();
            this.sn_textbox = new System.Windows.Forms.TextBox();
            this.model_textbox = new System.Windows.Forms.TextBox();
            this.model_text_label = new System.Windows.Forms.Label();
            this.model_label = new System.Windows.Forms.Label();
            this.sn_label = new System.Windows.Forms.Label();
            this.sn_picturebox = new System.Windows.Forms.PictureBox();
            this.camera1_timer = new System.Windows.Forms.Timer(this.components);
            this.camera2_timer = new System.Windows.Forms.Timer(this.components);
            this.camera3_timer = new System.Windows.Forms.Timer(this.components);
            this.scanner_timer = new System.Windows.Forms.Timer(this.components);
            this.camera1_groupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zoom1_trackbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.brightness1_trackbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.focus1_trackbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.camera1_picturebox)).BeginInit();
            this.unitinfo_groupbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.model_picturebox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sn_picturebox)).BeginInit();
            this.SuspendLayout();
            // 
            // camera1_groupbox
            // 
            this.camera1_groupbox.Controls.Add(this.focus_val_label);
            this.camera1_groupbox.Controls.Add(this.zoom1_val_label);
            this.camera1_groupbox.Controls.Add(this.brightness1_val_label);
            this.camera1_groupbox.Controls.Add(this.focus1_val_label);
            this.camera1_groupbox.Controls.Add(this.zoom1_label);
            this.camera1_groupbox.Controls.Add(this.zoom1_trackbar);
            this.camera1_groupbox.Controls.Add(this.brightness1_label);
            this.camera1_groupbox.Controls.Add(this.focus1_label);
            this.camera1_groupbox.Controls.Add(this.brightness1_trackbar);
            this.camera1_groupbox.Controls.Add(this.focus1_trackbar);
            this.camera1_groupbox.Controls.Add(this.camera1_picturebox);
            this.camera1_groupbox.Location = new System.Drawing.Point(12, 12);
            this.camera1_groupbox.Name = "camera1_groupbox";
            this.camera1_groupbox.Size = new System.Drawing.Size(811, 556);
            this.camera1_groupbox.TabIndex = 0;
            this.camera1_groupbox.TabStop = false;
            this.camera1_groupbox.Text = "Camera 1";
            // 
            // focus_val_label
            // 
            this.focus_val_label.AutoSize = true;
            this.focus_val_label.Location = new System.Drawing.Point(205, 511);
            this.focus_val_label.Name = "focus_val_label";
            this.focus_val_label.Size = new System.Drawing.Size(0, 13);
            this.focus_val_label.TabIndex = 10;
            // 
            // zoom1_val_label
            // 
            this.zoom1_val_label.AutoSize = true;
            this.zoom1_val_label.Location = new System.Drawing.Point(523, 300);
            this.zoom1_val_label.Name = "zoom1_val_label";
            this.zoom1_val_label.Size = new System.Drawing.Size(0, 13);
            this.zoom1_val_label.TabIndex = 9;
            // 
            // brightness1_val_label
            // 
            this.brightness1_val_label.AutoSize = true;
            this.brightness1_val_label.Location = new System.Drawing.Point(307, 300);
            this.brightness1_val_label.Name = "brightness1_val_label";
            this.brightness1_val_label.Size = new System.Drawing.Size(0, 13);
            this.brightness1_val_label.TabIndex = 8;
            // 
            // focus1_val_label
            // 
            this.focus1_val_label.AutoSize = true;
            this.focus1_val_label.Location = new System.Drawing.Point(74, 300);
            this.focus1_val_label.Name = "focus1_val_label";
            this.focus1_val_label.Size = new System.Drawing.Size(0, 13);
            this.focus1_val_label.TabIndex = 7;
            // 
            // zoom1_label
            // 
            this.zoom1_label.AutoSize = true;
            this.zoom1_label.Location = new System.Drawing.Point(616, 475);
            this.zoom1_label.Name = "zoom1_label";
            this.zoom1_label.Size = new System.Drawing.Size(34, 13);
            this.zoom1_label.TabIndex = 6;
            this.zoom1_label.Text = "Zoom";
            // 
            // zoom1_trackbar
            // 
            this.zoom1_trackbar.Location = new System.Drawing.Point(594, 511);
            this.zoom1_trackbar.Maximum = 7;
            this.zoom1_trackbar.Minimum = 1;
            this.zoom1_trackbar.Name = "zoom1_trackbar";
            this.zoom1_trackbar.Size = new System.Drawing.Size(173, 45);
            this.zoom1_trackbar.TabIndex = 5;
            this.zoom1_trackbar.Value = 4;
            this.zoom1_trackbar.Scroll += new System.EventHandler(this.zoom1_Slider);
            // 
            // brightness1_label
            // 
            this.brightness1_label.AutoSize = true;
            this.brightness1_label.Location = new System.Drawing.Point(307, 475);
            this.brightness1_label.Name = "brightness1_label";
            this.brightness1_label.Size = new System.Drawing.Size(56, 13);
            this.brightness1_label.TabIndex = 4;
            this.brightness1_label.Text = "Brightness";
            // 
            // focus1_label
            // 
            this.focus1_label.AutoSize = true;
            this.focus1_label.Location = new System.Drawing.Point(74, 475);
            this.focus1_label.Name = "focus1_label";
            this.focus1_label.Size = new System.Drawing.Size(36, 13);
            this.focus1_label.TabIndex = 3;
            this.focus1_label.Text = "Focus";
            // 
            // brightness1_trackbar
            // 
            this.brightness1_trackbar.Location = new System.Drawing.Point(310, 511);
            this.brightness1_trackbar.Maximum = 100;
            this.brightness1_trackbar.Name = "brightness1_trackbar";
            this.brightness1_trackbar.Size = new System.Drawing.Size(213, 45);
            this.brightness1_trackbar.TabIndex = 2;
            this.brightness1_trackbar.TickFrequency = 10;
            this.brightness1_trackbar.Scroll += new System.EventHandler(this.brightness1_Scroll);
            // 
            // focus1_trackbar
            // 
            this.focus1_trackbar.Location = new System.Drawing.Point(14, 511);
            this.focus1_trackbar.Maximum = 100;
            this.focus1_trackbar.Name = "focus1_trackbar";
            this.focus1_trackbar.Size = new System.Drawing.Size(173, 45);
            this.focus1_trackbar.TabIndex = 1;
            this.focus1_trackbar.TickFrequency = 10;
            this.focus1_trackbar.Scroll += new System.EventHandler(this.focus1_Scroll);
            // 
            // camera1_picturebox
            // 
            this.camera1_picturebox.Location = new System.Drawing.Point(14, 19);
            this.camera1_picturebox.Name = "camera1_picturebox";
            this.camera1_picturebox.Size = new System.Drawing.Size(753, 435);
            this.camera1_picturebox.TabIndex = 0;
            this.camera1_picturebox.TabStop = false;
            // 
            // unitinfo_groupbox
            // 
            this.unitinfo_groupbox.Controls.Add(this.model_picturebox);
            this.unitinfo_groupbox.Controls.Add(this.sn_textbox);
            this.unitinfo_groupbox.Controls.Add(this.model_textbox);
            this.unitinfo_groupbox.Controls.Add(this.model_text_label);
            this.unitinfo_groupbox.Controls.Add(this.model_label);
            this.unitinfo_groupbox.Controls.Add(this.sn_label);
            this.unitinfo_groupbox.Controls.Add(this.sn_picturebox);
            this.unitinfo_groupbox.Location = new System.Drawing.Point(75, 613);
            this.unitinfo_groupbox.Name = "unitinfo_groupbox";
            this.unitinfo_groupbox.Size = new System.Drawing.Size(1156, 160);
            this.unitinfo_groupbox.TabIndex = 3;
            this.unitinfo_groupbox.TabStop = false;
            this.unitinfo_groupbox.Text = "Unit Info";
            // 
            // model_picturebox
            // 
            this.model_picturebox.Location = new System.Drawing.Point(1029, 59);
            this.model_picturebox.Name = "model_picturebox";
            this.model_picturebox.Size = new System.Drawing.Size(68, 71);
            this.model_picturebox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.model_picturebox.TabIndex = 6;
            this.model_picturebox.TabStop = false;
            // 
            // sn_textbox
            // 
            this.sn_textbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sn_textbox.Location = new System.Drawing.Point(40, 68);
            this.sn_textbox.Name = "sn_textbox";
            this.sn_textbox.Size = new System.Drawing.Size(386, 47);
            this.sn_textbox.TabIndex = 5;
            // 
            // model_textbox
            // 
            this.model_textbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.model_textbox.Location = new System.Drawing.Point(616, 68);
            this.model_textbox.Name = "model_textbox";
            this.model_textbox.Size = new System.Drawing.Size(385, 47);
            this.model_textbox.TabIndex = 4;
            // 
            // model_text_label
            // 
            this.model_text_label.AutoSize = true;
            this.model_text_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.model_text_label.Location = new System.Drawing.Point(33, 273);
            this.model_text_label.Name = "model_text_label";
            this.model_text_label.Size = new System.Drawing.Size(0, 39);
            this.model_text_label.TabIndex = 3;
            // 
            // model_label
            // 
            this.model_label.AutoSize = true;
            this.model_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.model_label.Location = new System.Drawing.Point(610, 32);
            this.model_label.Name = "model_label";
            this.model_label.Size = new System.Drawing.Size(94, 33);
            this.model_label.TabIndex = 2;
            this.model_label.Text = "Model";
            // 
            // sn_label
            // 
            this.sn_label.AutoSize = true;
            this.sn_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sn_label.Location = new System.Drawing.Point(77, 32);
            this.sn_label.Name = "sn_label";
            this.sn_label.Size = new System.Drawing.Size(202, 33);
            this.sn_label.TabIndex = 1;
            this.sn_label.Text = "Serial Number";
            // 
            // sn_picturebox
            // 
            this.sn_picturebox.Location = new System.Drawing.Point(448, 59);
            this.sn_picturebox.Name = "sn_picturebox";
            this.sn_picturebox.Size = new System.Drawing.Size(71, 67);
            this.sn_picturebox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.sn_picturebox.TabIndex = 0;
            this.sn_picturebox.TabStop = false;
            // 
            // camera1_timer
            // 
            this.camera1_timer.Interval = 33;
            this.camera1_timer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // scanner_timer
            // 
            this.scanner_timer.Interval = 50;
            this.scanner_timer.Tick += new System.EventHandler(this.scanner_timer_Tick);
            // 
            // CameraView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1386, 785);
            this.Controls.Add(this.unitinfo_groupbox);
            this.Controls.Add(this.camera1_groupbox);
            this.KeyPreview = true;
            this.Name = "CameraView";
            this.Text = "CameraView";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Camera_FormClosing);
            this.Load += new System.EventHandler(this.Camera_form_load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Sensor_Triggered);
            this.camera1_groupbox.ResumeLayout(false);
            this.camera1_groupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.zoom1_trackbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.brightness1_trackbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.focus1_trackbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.camera1_picturebox)).EndInit();
            this.unitinfo_groupbox.ResumeLayout(false);
            this.unitinfo_groupbox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.model_picturebox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sn_picturebox)).EndInit();
            this.ResumeLayout(false);

        }

        private async void Sensor_Triggered(object sender, KeyPressEventArgs e)
        {
            if (!e.KeyChar.Equals('+')) return;

            sn_textbox.Text = "";
            model_textbox.Text = "";
            using var foundCts = new CancellationTokenSource();
            using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(foundCts.Token, timeoutCts.Token);

            var foundTaskCompletionSource = new TaskCompletionSource<(string, string)>(TaskCreationOptions.RunContinuationsAsynchronously);

            scanner_timer.Start();

            try
            {
                var worker = Task.Run( () => RunCameraProcess(linkedCts.Token, foundCts, foundTaskCompletionSource), linkedCts.Token);

                var completed = await Task.WhenAny(foundTaskCompletionSource.Task,
                    Task.Delay(Timeout.Infinite, timeoutCts.Token))
                    .ConfigureAwait(true);

                if(completed == foundTaskCompletionSource.Task)
                {
                    var results = await foundTaskCompletionSource.Task;
                    sn_textbox.Text = results.Item1;
                    model_textbox.Text = results.Item2;
                }
                else
                {
                    model_textbox.Text = "Not Found";
                }
            }
            catch(OperationCanceledException) { }
            finally
            {
                scanner_timer.Stop();
                foundCts.Cancel();
                timeoutCts.Cancel();
            }
        }

        #endregion

        private System.Windows.Forms.GroupBox camera1_groupbox;
        private System.Windows.Forms.GroupBox unitinfo_groupbox;
        private System.Windows.Forms.PictureBox camera1_picturebox;
        private System.Windows.Forms.Label model_label;
        private System.Windows.Forms.Label sn_label;
        private System.Windows.Forms.PictureBox sn_picturebox;
        private System.Windows.Forms.Label brightness1_label;
        private System.Windows.Forms.Label focus1_label;
        private System.Windows.Forms.TrackBar brightness1_trackbar;
        private System.Windows.Forms.TrackBar focus1_trackbar;
        private System.Windows.Forms.Timer camera1_timer;
        private System.Windows.Forms.Timer camera2_timer;
        private System.Windows.Forms.Timer camera3_timer;
        private System.Windows.Forms.Timer scanner_timer;
        private System.Windows.Forms.Label zoom1_label;
        private System.Windows.Forms.TrackBar zoom1_trackbar;
        private Label zoom1_val_label;
        private Label brightness1_val_label;
        private Label focus1_val_label;
        private Label model_text_label;
        private TextBox sn_textbox;
        private TextBox model_textbox;
        private PictureBox model_picturebox;
        private Label focus_val_label;
    }
}