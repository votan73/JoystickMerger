using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoystickMerger.Generator
{
    class Generator : System.CodeDom.Compiler.TempFileCollection
    {
        static string GetLocation()
        {
            return Path.GetDirectoryName(typeof(MainForm).Assembly.Location);
        }
        public Generator()
            : base(Path.Combine(GetLocation(), "Build"), true)
        {
        }

        readonly static Dictionary<string, string> lookup = new Dictionary<string, string>();
        static Generator()
        {
            lookup["AxisX"] = "bool AxisX = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_X);";
            lookup["AxisY"] = "bool AxisY = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_Y);";
            lookup["AxisZ"] = "bool AxisZ = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_Z);";
            lookup["AxisXRot"] = "bool AxisXRot = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_RX);";
            lookup["AxisYRot"] = "bool AxisYRot = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_RY);";
            lookup["AxisZRot"] = "bool AxisZRot = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_RZ);";
            lookup["AxisSL0"] = "bool AxisSL0 = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_SL0);";
            lookup["AxisSL1"] = "bool AxisSL1 = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_SL1);";
            lookup["AxisWHL"] = "bool AxisWHL = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_WHL);";
            lookup["AxisPOV"] = "bool AxisPOV = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_POV);";
        }

        public bool Build(params IMapItem[] items)
        {
            var location = GetLocation();
            CopyTemplateToTempFolder(location);


            var info = new CompileInfo();
            foreach (var item in items)
                item.Initialize(info);
            info.EndInitialization();

            using (var file = File.CreateText(System.IO.Path.Combine(TempDir, "JoystickMerger.Feeder", "GameDevPoller.Generated.cs")))
            {
                file.WriteLine("using System;");
                file.WriteLine("using System.Collections.Generic;");
                file.WriteLine("using System.Linq;");
                file.WriteLine("using System.Text;");
                file.WriteLine("using SharpDX;");
                file.WriteLine("using SharpDX.DirectInput;");
                file.WriteLine("using System.Windows.Forms;");
                file.WriteLine("using vJoyInterfaceWrap;");
                file.WriteLine();
                file.WriteLine("// This file is generated with JoystickMerger.Generator. Do not edit.");
                file.WriteLine("namespace JoystickMerger.Feeder");
                file.WriteLine("{");
                file.WriteLine("    partial class GameDevPoller");
                file.WriteLine("    {");
                file.WriteLine("        // vJoy device number.");
                file.WriteLine("        const uint id = 1;");

                foreach (var item in items)
                    item.Declaration(info, file);
                file.WriteLine();

                ValidateVJoyConfiguration(info, file);

                foreach (var item in items)
                    item.PreFeed(info, file);
                file.WriteLine("        private void Feed()");
                file.WriteLine("        {");
                foreach (var item in items)
                    item.Feed(info, file);
                file.WriteLine("        }");
                file.WriteLine();

                foreach (var item in items)
                    item.PostFeed(info, file);

                file.WriteLine("    }");
                file.WriteLine("}");
            }


            return RunCompiler();
        }

        private void ValidateVJoyConfiguration(CompileInfo info, StreamWriter file)
        {
            file.WriteLine("        private bool ValidateVJoyConfiguration()");
            file.WriteLine("        {");
            //foreach (var name in Enum.GetNames(typeof(HID_USAGES)))
            //{
            //    file.Write("lookup[\"Axis");
            //    file.Write(name.Replace("HID_USAGE_", ""));
            //    file.Write("\"] = \"bool Axis");
            //    file.Write(name.Replace("HID_USAGE_", ""));
            //    file.Write(" = joystick.GetVJDAxisExist(id, HID_USAGES.");
            //    file.Write(name);
            //    file.WriteLine(");\";");
            //}

            file.WriteLine("            // Make sure all needed axes and buttons are supported");
            var list = new List<string>();
            var axis = info.GetVJoyAxis();
            foreach (var vJoyAxis in axis)
            {
                file.Write("            ");
                file.WriteLine(lookup[vJoyAxis]);
                list.Add("!" + vJoyAxis);
            }
            list.Add("nButtons < " + info.GetMaxButtons());
            list.Add("cont < " + info.GetMaxPOVs());

            file.WriteLine("            int nButtons = joystick.GetVJDButtonNumber(id);");
            file.WriteLine("            int cont = joystick.GetVJDContPovNumber(id);");

            file.Write("            if (");
            file.Write(String.Join(" || ", list));
            file.WriteLine(")");
            file.WriteLine("            {");
            file.Write("                mainForm.ReportError(\"vJoy Device is not configured correctly. Must have ");
            file.Write(String.Join(", ", axis));
            file.Write(" analog axis, ");
            file.Write(info.GetMaxButtons());
            file.Write(" buttons and ");
            file.Write(info.GetMaxPOVs());
            file.WriteLine(" analog POVs. Cannot continue\\n\");");
            file.WriteLine("                return false;");
            file.WriteLine("            }");
            file.WriteLine("            return true;");
            file.WriteLine("        }");
            file.WriteLine();
        }

        private bool RunCompiler()
        {
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = Path.Combine(Path.GetDirectoryName(typeof(GC).Assembly.Location), "MSBuild.exe");
            process.StartInfo.WorkingDirectory = Path.Combine(this.TempDir, "JoystickMerger.Feeder");
            process.StartInfo.Arguments = "JoystickMerger.Feeder.csproj /p:\"Configuration=Release\" /p:\"Platform=x64\" /nologo";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();

            var output = process.StandardOutput.ReadToEnd();
            var err = process.StandardError.ReadToEnd();
            process.WaitForExit();

            Output = process.ExitCode != 0 ? output : "";

            return process.ExitCode == 0;
        }

        private void CopyTemplateToTempFolder(string location)
        {
            var parent = Path.GetDirectoryName(location);
            var commonPath = Path.Combine(parent, "Common");
            var playerPath = Path.Combine(parent, "JoystickMerger.Feeder");
            var packagesPath = Path.Combine(parent, "packages");

            if (!Directory.Exists(commonPath))
                throw new DirectoryNotFoundException(String.Concat("Path \"", commonPath, "\" missing."));
            if (!Directory.Exists(playerPath))
                throw new DirectoryNotFoundException(String.Concat("Path \"", playerPath, "\" missing."));
            if (!Directory.Exists(packagesPath))
                throw new DirectoryNotFoundException(String.Concat("Path \"", packagesPath, "\" missing."));

            CopyFolder(commonPath, this.TempDir);
            CopyFolder(playerPath, this.TempDir);
            CopyFolder(packagesPath, this.TempDir);
        }

        private void CopyFolder(string SourcePath, string DestinationPath)
        {
            if (SourcePath == DestinationPath)
                return;

            int indexSrc = Path.GetDirectoryName(SourcePath).Length + 1;

            foreach (var path in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories))
            {
                var dest = System.IO.Path.Combine(DestinationPath, path.Substring(indexSrc));
                Directory.CreateDirectory(Path.GetDirectoryName(dest));
                File.Copy(path, dest, true);
            }
        }

        public string Output;
    }
}
