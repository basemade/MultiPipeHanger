using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Autodesk.Windows; //Need reference: AdWindows dll, from C:\Program Files\Autodesk\Revit yyyy
using Microsoft.VisualBasic.CompilerServices;


namespace Test_01_03
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
	[Regeneration(RegenerationOption.Manual)]
	public class Test06_RubberBandLine_rewrite : IExternalCommand
    {
		private IntPtr _revit_window;
		private List<ElementId> _added_element_ids;

		public Test06_RubberBandLine_rewrite()
		{
			_added_element_ids = new List<ElementId>();
		}
		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			UIApplication application = commandData.Application;
			UIDocument activeUIDocument = application.ActiveUIDocument;
			Application application2 = application.Application;
			Document document = activeUIDocument.Document;

			_revit_window = application.MainWindowHandle;

			FilteredElementCollector val = new FilteredElementCollector(document);
			val.OfCategory((BuiltInCategory)(-2002000));//OST_DetailComponents = -2002000
			val.OfClass(typeof(FamilySymbol));
			FamilySymbol val2 = (from FamilySymbol tag in (IEnumerable)new FilteredElementCollector(document).OfClass(typeof(FamilySymbol)).OfCategory((BuiltInCategory)(-2002000))
								 where Operators.CompareString(((Element)tag).Name, "RubberBand", TextCompare: false) == 0
								 select (tag)).First();
			//上面這是LINQ語法

			_added_element_ids.Clear();
			application2.DocumentChanged += new EventHandler<DocumentChangedEventArgs>(OnDocumentChanged);

			try
			{
				activeUIDocument.PromptForFamilyInstancePlacement(val2);
			}
			catch (Autodesk.Revit.Exceptions.OperationCanceledException val3)
			{
				ProjectData.SetProjectError((Exception)val3);
				
				Autodesk.Revit.Exceptions.OperationCanceledException val4 = val3;
				Debug.Print(((Exception)(object)val4).Message);
				ProjectData.ClearProjectError();
			}

			application2.DocumentChanged -= new EventHandler<DocumentChangedEventArgs>(OnDocumentChanged);
			return Result.Succeeded;
		}
		private void OnDocumentChanged(object sender, DocumentChangedEventArgs e)
		{
			ICollection<ElementId> addedElementIds = e.GetAddedElementIds();
			int count = addedElementIds.Count;
			_added_element_ids.AddRange(addedElementIds);
			if (_added_element_ids.Count >= 1 && ComponentManager.ApplicationWindow != IntPtr.Zero)
			{
				//WindowsMessaging.PostWindowsMessage((int)ComponentManager.ApplicationWindow, 256, 27, 0);
				//WindowsMessaging.PostWindowsMessage((int)ComponentManager.ApplicationWindow, 256, 27, 0);
				//原程式有兩行，可以創建一次線段

				WindowsMessaging.PostWindowsMessage((int)ComponentManager.ApplicationWindow, 256, 27, 0);
				//如果修改成只有一行，可以創建多次線段

			}
		}

	}
}
