using AutoZilla.Core;
using AutoZilla.Core.Extensions;
using AutoZilla.Core.GlobalHotKeys;
using AutoZilla.Core.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoZilla.Plugins.MSSQL
{
    public class MSSQLPlugin : IAutoZillaPlugin
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IGlobalHotKeyManager HKM;
        TextOutputter TOUT = new TextOutputter();
        TextTemplate HeaderCommentTemplate;
        string TemplateFolder;

        public void InitialiseHotKeyManager(IGlobalHotKeyManager manager)
        {
            HKM = manager;
            log.Debug("Initialising MSSQLPlugin.");

            TemplateFolder = TemplateLoader.GetDefaultTemplateFolder(Assembly.GetExecutingAssembly());
            LoadAutoTemplates();
            LoadCustomTemplates();
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
                HKM.Register(template);
            }
        }

        void LoadCustomTemplates()
        {
            HeaderCommentTemplate = TemplateLoader.LoadTemplate(Path.Combine(TemplateFolder, "HeaderComment.azt"));
            HKM.Register(HeaderCommentTemplate.ModifiedKey, OutputHeaderComment);
        }

        void OutputHeaderComment(ModifiedKey key)
        {
            string text = HeaderCommentTemplate.Process();

            TOUT.WaitForModifiersUp();
            TOUT.PasteString(text).
                Move(-9, 3).
                SelectToEndOfLine();
        }
    }
}
