#define DEBUG
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.Exceptions;
using Autodesk.Revit.UI;
using Autodesk.Windows;
using Microsoft.VisualBasic.CompilerServices;
//using RubberBand;
//using static RubberBand;


//[Transaction(/*Could not decode attribute arguments.*/)]
//[Regeneration(/*Could not decode attribute arguments.*/)]
[Transaction(TransactionMode.Manual)]
[Regeneration(RegenerationOption.Manual)]
internal class Test05_RubberBandLine_2 : IExternalCommand
{
	//private static bool _place_one_single_instance_then_abort = true;

	private IntPtr _revit_window;

	private List<ElementId> _added_element_ids;

	//private object picClicker;

	public Test05_RubberBandLine_2()
	{
		_added_element_ids = new List<ElementId>();
	}

	public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Expected O, but got Unknown
		//IL_0103: Expected O, but got Unknown
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)


		//UIApplication application = commandData.get_Application();
		//UIDocument activeUIDocument = application.get_ActiveUIDocument();
		//Application application2 = application.get_Application();
		//Document document = activeUIDocument.get_Document();
		UIApplication application = commandData.Application;
		UIDocument activeUIDocument = application.ActiveUIDocument;
		Application application2 = application.Application;
		Document document = activeUIDocument.Document;
		
		//_revit_window = application.get_MainWindowHandle();
		_revit_window = application.MainWindowHandle;

		FilteredElementCollector val = new FilteredElementCollector(document);
		val.OfCategory((BuiltInCategory)(-2002000));//OST_DetailComponents = -2002000
		val.OfClass(typeof(FamilySymbol));
		FamilySymbol val2 = (from FamilySymbol tag in (IEnumerable)new FilteredElementCollector(document).OfClass(typeof(FamilySymbol)).OfCategory((BuiltInCategory)(-2002000))
							 //where Operators.CompareString(((Element)tag).get_Name(), "RubberBand", TextCompare: false) == 0
							 where Operators.CompareString(((Element)tag).Name, "RubberBand", TextCompare: false) == 0
							 select (tag)).First();
		_added_element_ids.Clear();

		//application2.add_DocumentChanged((EventHandler<DocumentChangedEventArgs>)OnDocumentChanged);
		application2.DocumentChanged += new EventHandler<DocumentChangedEventArgs>(OnDocumentChanged);
		
		try
		{
			activeUIDocument.PromptForFamilyInstancePlacement(val2);
		}
		//catch (OperationCanceledException val3)
		catch (Autodesk.Revit.Exceptions.OperationCanceledException val3)
		{
			ProjectData.SetProjectError((Exception)val3);
			//OperationCanceledException val4 = val3;
			Autodesk.Revit.Exceptions.OperationCanceledException val4 = val3;
			Debug.Print(((Exception)(object)val4).Message);
			ProjectData.ClearProjectError();
		}

		//application2.remove_DocumentChanged((EventHandler<DocumentChangedEventArgs>)OnDocumentChanged);
		application2.DocumentChanged -= new EventHandler<DocumentChangedEventArgs> (OnDocumentChanged);

		int count = _added_element_ids.Count;
		//TaskDialog.Show("Place Family Instance", string.Format("{0} element{1} added.", count, (1 == count) ? "" : "s"));
		Autodesk.Revit.UI.TaskDialog.Show("Place Family Instance", string.Format("{0} element{1} added.", count, (1 == count) ? "" : "s"));
		
		/*
		foreach (ElementId added_element_id in _added_element_ids)
		{
			string value = added_element_id.ToString();
			int num = Convert.ToInt32(value);
			ElementId val5 = new ElementId(num);
			_variables.eFromId = document.GetElement(val5);
			//TaskDialog.Show("Place Family Instance", _variables.eFromId.get_Name().ToString() + "---" + val5.ToString());
			Autodesk.Revit.UI.TaskDialog.Show("Place Family Instance", _variables.eFromId.Name.ToString() + "---" + val5.ToString());
			_variables.p = _variables.eFromId.get_Parameter((BuiltInParameter)(-1001306));
		}*/

		/*
		Form1 form = new Form1(commandData);
		//form.lbl_tipo.Text = _variables.eFromId.get_Name() + "-----" + _variables.p.AsValueString();
		form.lbl_tipo.Text = _variables.eFromId.Name + "-----" + _variables.p.AsValueString();
		form.ShowDialog();
		*/
		return (Result)0;
		
	}

	private void OnDocumentChanged(object sender, DocumentChangedEventArgs e)
	{
		ICollection<ElementId> addedElementIds = e.GetAddedElementIds();
		int count = addedElementIds.Count;
		_added_element_ids.AddRange(addedElementIds);
		//if (_added_element_ids.Count >= 1 && ComponentManager.get_ApplicationWindow() != IntPtr.Zero)
		if (_added_element_ids.Count >= 1 && ComponentManager.ApplicationWindow != IntPtr.Zero)
		{
			//WindowsMessaging.PostWindowsMessage((int)ComponentManager.get_ApplicationWindow(), 256, 27, 0);
			//WindowsMessaging.PostWindowsMessage((int)ComponentManager.get_ApplicationWindow(), 256, 27, 0);

			//WindowsMessaging.PostWindowsMessage((int)ComponentManager.ApplicationWindow, 256, 27, 0);
			//WindowsMessaging.PostWindowsMessage((int)ComponentManager.ApplicationWindow, 256, 27, 0);
			//原程式有兩行"WindowsMessaging.PostWindowsMessage((int)ComponentManager.ApplicationWindow, 256, 27, 0)"
			//可以創建一次線段

			WindowsMessaging.PostWindowsMessage((int)ComponentManager.ApplicationWindow, 256, 27, 0);
			//如果修改成只有一行，可以創建多次線段

		}
	}
}