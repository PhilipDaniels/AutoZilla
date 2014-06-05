using AutoZilla.Core;
using AutoZilla.Core.GlobalHotKeys;
using AutoZilla.Core.Templates;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace AutoZilla.Plugins.MSSQL
{
    public class MSSQLPlugin : IAutoZillaPlugin
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IGlobalHotKeyManager HKM;
        TextOutputter TOUT = new TextOutputter();
        string TemplateFolder;
        List<TextTemplate> AutoTemplates;
        TextTemplate HeaderCommentTemplate;
        TextTemplate CreateTableTemplate;
        MainForm MainForm;

        public void InitialiseHotKeyManager(IGlobalHotKeyManager manager)
        {
            HKM = manager;
            log.Debug("Initialising MSSQLPlugin.");

            TemplateFolder = TemplateLoader.GetDefaultTemplateFolder(Assembly.GetExecutingAssembly());
            AutoTemplates = new List<TextTemplate>();
            LoadAutoTemplates();
            LoadCustomTemplates();
            HKM.Register(Modifiers.Ctrl | Modifiers.Shift, Keys.P, ShowMainForm);
        }

        void LoadAutoTemplates()
        {
            LoadAutoTemplate("CountStar.azt");
            LoadAutoTemplate("ReadUnCommitted.azt");
            LoadAutoTemplate("SelectStar.azt");
            LoadAutoTemplate("TopStar.azt");
        }

        void LoadAutoTemplate(string filename)
        {
            string templateFilePath = Path.Combine(TemplateFolder, filename);
            var template = TemplateLoader.LoadTemplate(templateFilePath);
            if (template != null)
            {
                AutoTemplates.Add(template);
                HKM.Register(template);
            }
        }

        void LoadCustomTemplates()
        {
            HeaderCommentTemplate = TemplateLoader.LoadTemplate(Path.Combine(TemplateFolder, "HeaderComment.azt"));
            HKM.Register(HeaderCommentTemplate.ModifiedKey, OutputHeaderComment);

            CreateTableTemplate = TemplateLoader.LoadTemplate(Path.Combine(TemplateFolder, "CreateTable.azt"));
        }

        void OutputHeaderComment(ModifiedKey key)
        {
            string text = HeaderCommentTemplate.Process();

            TOUT.WaitForModifiersUp();
            TOUT.PasteString(text).
                Move(-9, 3).
                SelectToEndOfLine();
        }

        void ShowMainForm(ModifiedKey key)
        {
            // Ensure form only shown once.
            if (MainForm != null)
                return;

            using (var f = new MainForm())
            {
                MainForm = f;
                f.AutoTemplates = AutoTemplates;
                f.ShowDialog();
                MainForm = null;
            }
        }
    }
}
