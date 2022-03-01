using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Microsoft.Toolkit.Uwp.UI;

namespace SqlDBObjects
{
	public class EnumToStringConverter : Windows.UI.Xaml.Data.IValueConverter
	{
		public System.Object Convert(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			System.String strResult = null;
			System.Type sourceType = null;
			if(value != null && (sourceType = value.GetType()).IsEnum && targetType.IsAssignableFrom(typeof(System.String)))
			{
				strResult = System.Enum.GetName(sourceType, value);
			}
			return strResult;
		}
		public System.Object ConvertBack(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			System.Object enumValue = null;
			System.Type sourceType = null;
			if(value != null && (sourceType = value.GetType()) == typeof(System.String) && targetType.IsEnum)
			{
				enumValue = System.Enum.Parse(targetType, (System.String)value);
			}
			return enumValue;
		}
	};

	//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	public class UInt16ToStringConverter : Windows.UI.Xaml.Data.IValueConverter
	{
		public System.Object Convert(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			System.String strResult = null;
			if(value != null && typeof(ushort).IsAssignableFrom(value.GetType()) && targetType == typeof(System.String))
			{
				ushort iValue = System.Convert.ToUInt16(value);
				strResult = System.Convert.ToString(iValue);
			}
			return strResult;
		}
		public System.Object ConvertBack(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			ushort iResult = 0;
			if(value != null && value.GetType() == typeof(System.String) && targetType.IsAssignableFrom(typeof(System.UInt16)))
			{
				System.String strValue = (System.String)value;
				iResult = System.String.IsNullOrEmpty(strValue) ? System.Convert.ToUInt16(0) : System.Convert.ToUInt16(strValue);
			}
			return iResult;
		}
	};

	//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	public class UInt16ToDoubleConverter : Windows.UI.Xaml.Data.IValueConverter
	{
		public System.Object Convert(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			double dResult = Double.NaN;
			if(value != null && typeof(System.UInt16).IsAssignableFrom(value.GetType()) && targetType.IsAssignableFrom(typeof(double)))
			{
				ushort iValue = System.Convert.ToUInt16(value);
				dResult = System.Convert.ToDouble(iValue);
			}
			return dResult;
		}
		public System.Object ConvertBack(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			ushort iResult = 0;
			if(value != null && typeof(double).IsAssignableFrom(value.GetType()) && targetType.IsAssignableFrom(typeof(System.UInt16)))
			{
				double dValue = System.Convert.ToDouble(value);
				if(dValue != Double.NaN && dValue != Double.NegativeInfinity && dValue != Double.PositiveInfinity)
				{
					iResult = System.Convert.ToUInt16(dValue);
				}
			}
			return iResult;
		}
	};

	//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	public class Int32ToStringConverter : Windows.UI.Xaml.Data.IValueConverter
	{
		public System.Object Convert(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			System.String strResult = null;
			if(value != null && typeof(System.Int32).IsAssignableFrom(value.GetType()) && targetType == typeof(System.String))
			{
				int nValue = System.Convert.ToInt32(value);
				strResult = System.Convert.ToString(nValue);
			}
			return strResult;
		}
		public System.Object ConvertBack(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			int nResult = 0;
			if(value != null && value.GetType() == typeof(System.String) && targetType.IsAssignableFrom(typeof(System.Int32)))
			{
				System.String strValue = (System.String)value;
				nResult = System.String.IsNullOrEmpty(strValue) ? 0 : System.Convert.ToInt32(strValue);
			}
			return nResult;
		}
	};

	//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	public class Int32EqualityConverter : Windows.UI.Xaml.Data.IValueConverter
	{
		public System.Object Convert(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			bool fResult = false;
			if(value != null && typeof(int).IsAssignableFrom(value.GetType()) && targetType.IsAssignableFrom(typeof(bool))
				&& (parameter == null || parameter.GetType().GetInterface("IConvertible") != null))
			{
				int nValue1 = System.Convert.ToInt32(value);
				int nValue2 = parameter != null ? System.Convert.ToInt32(parameter) : 0;
				fResult = nValue1 == nValue2;
			}
			return fResult;
		}
		public System.Object ConvertBack(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			return null;
		}
	};

	//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	public class Int32NonEqualityConverter : Windows.UI.Xaml.Data.IValueConverter
	{
		public System.Object Convert(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			bool fResult = false;
			if(value != null && typeof(int).IsAssignableFrom(value.GetType()) && targetType.IsAssignableFrom(typeof(bool))
				&& (parameter == null || parameter.GetType().GetInterface("IConvertible") != null))
			{
				int nValue1 = System.Convert.ToInt32(value);
				int nValue2 = parameter != null ? System.Convert.ToInt32(parameter) : 0;
				fResult = nValue1 != nValue2;
			}
			return fResult;
		}
		public System.Object ConvertBack(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			return null;
		}
	};

	//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	public class Int32ComparisonConverter : Windows.UI.Xaml.Data.IValueConverter
	{
		public System.Object Convert(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			int nResult = -2;
			if(value != null && typeof(int).IsAssignableFrom(value.GetType()) && targetType.IsAssignableFrom(typeof(int))
				&& (parameter == null || typeof(int).IsAssignableFrom(parameter.GetType())))
			{
				int nValue1 = System.Convert.ToInt32(value);
				int nValue2 = parameter != null ? System.Convert.ToInt32(parameter) : 0;
				nResult = nValue1 == nValue2 ? 0 : (nValue1 < nValue2 ? -1 : 1);
			}
			return nResult;
		}
		public System.Object ConvertBack(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			return null;
		}
	};

	//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	public class DoubleToStringConverter : Windows.UI.Xaml.Data.IValueConverter
	{
		public System.Object Convert(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			System.String strResult = null;
			if(value != null && typeof(double).IsAssignableFrom(value.GetType()) && targetType == typeof(System.String))
			{
				double dValue = System.Convert.ToDouble(value);
				if(dValue != Double.NaN && dValue != Double.NegativeInfinity && dValue != Double.PositiveInfinity)
				{
					strResult = System.Convert.ToString(dValue);
				}
			}
			return strResult;
		}
		public System.Object ConvertBack(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			double dResult = Double.NaN;
			if(value != null && value.GetType() == typeof(System.String) && targetType.IsAssignableFrom(typeof(double)))
			{
				System.String strValue = (System.String)value;
				dResult = System.String.IsNullOrEmpty(strValue) ? 0 : System.Convert.ToDouble(strValue);
			}
			return dResult;
		}
	};

	//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	public class DoubleEqualityConverter : Windows.UI.Xaml.Data.IValueConverter
	{
		public System.Object Convert(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			bool fResult = false;
			if(value != null && typeof(double).IsAssignableFrom(value.GetType()) && targetType.IsAssignableFrom(typeof(bool))
				&& (parameter == null || typeof(double).IsAssignableFrom(parameter.GetType())))
			{
				double dValue1 = System.Convert.ToDouble(value);
				double dValue2 = parameter != null ? System.Convert.ToDouble(parameter) : 0;
				fResult = dValue1 == dValue2;
			}
			return fResult;
		}
		public System.Object ConvertBack(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			return null;
		}
	};

	//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	public class DoubleNonEqualityConverter : Windows.UI.Xaml.Data.IValueConverter
	{
		public System.Object Convert(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			bool fResult = false;
			if(value != null && typeof(double).IsAssignableFrom(value.GetType()) && targetType.IsAssignableFrom(typeof(bool))
				&& (parameter == null || parameter.GetType().GetInterface("IConvertible") != null))
			{
				double dValue1 = System.Convert.ToDouble(value);
				double dValue2 = parameter != null ? System.Convert.ToDouble(parameter) : 0;
				fResult = dValue1 != dValue2;
			}
			return fResult;
		}
		public System.Object ConvertBack(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			return null;
		}
	};

	//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	public class DoubleComparisonConverter : Windows.UI.Xaml.Data.IValueConverter
	{
		public System.Object Convert(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			double dResult = -2;
			if(value != null && typeof(double).IsAssignableFrom(value.GetType()) && targetType.IsAssignableFrom(typeof(double))
				&& (parameter == null || typeof(double).IsAssignableFrom(parameter.GetType())))
			{
				double dValue1 = System.Convert.ToDouble(value);
				double dValue2 = parameter != null ? System.Convert.ToDouble(parameter) : 0;
				dResult = dValue1 == dValue2 ? 0 : (dValue1 < dValue2 ? -1 : 1);
			}
			return dResult;
		}
		public System.Object ConvertBack(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			return null;
		}
	};

	//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	public class EmptyStringToBooleanConverter : Windows.UI.Xaml.Data.IValueConverter
	{
		public System.Object Convert(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			bool fResult = false;
			if((value == null || typeof(System.String).IsAssignableFrom(value.GetType())) && targetType.IsAssignableFrom(typeof(bool)))
			{
				fResult = System.String.IsNullOrEmpty((System.String)value);
			}
			return fResult;
		}
		public System.Object ConvertBack(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			return null;
		}
	};

	//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	public class NotEmptyStringToBooleanConverter : Windows.UI.Xaml.Data.IValueConverter
	{
		public System.Object Convert(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			bool fResult = false;
			if((value == null || typeof(System.String).IsAssignableFrom(value.GetType())) && targetType.IsAssignableFrom(typeof(bool)))
			{
				fResult = !System.String.IsNullOrEmpty((System.String)value);
			}
			return fResult;
		}
		public System.Object ConvertBack(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			return null;
		}
	};

	//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	public class NullToBooleanConverter : Windows.UI.Xaml.Data.IValueConverter
	{
		public System.Object Convert(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			return value == null;
		}
		public System.Object ConvertBack(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			return null;
		}
	};

	//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	public class BooleanLogicalNotConverter : Windows.UI.Xaml.Data.IValueConverter
	{
		public System.Object Convert(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			bool fResult = false;
			if(value != null && typeof(bool).IsAssignableFrom(value.GetType()) && targetType.IsAssignableFrom(typeof(bool)))
			{
				bool fValue = System.Convert.ToBoolean(value);
				fResult = !fValue;
			}
			return fResult;
		}
		public System.Object ConvertBack(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			bool fResult = false;
			if(value != null && typeof(bool).IsAssignableFrom(value.GetType()) && targetType.IsAssignableFrom(typeof(bool)))
			{
				bool fValue = System.Convert.ToBoolean(value);
				fResult = !fValue;
			}
			return fResult;
		}
	};

	//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	public class FilePathToStringContentConverter : Windows.UI.Xaml.Data.IValueConverter
	{
		public System.Object Convert(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			System.String strResult = null;
			if(value != null && typeof(System.String).IsAssignableFrom(value.GetType()) && targetType == typeof(System.String))
			{
				System.String strFilePath = (System.String)value;
				if(!System.String.IsNullOrEmpty(strFilePath))
				{
					try
					{
						strResult = System.IO.File.ReadAllText(strFilePath);
					}
					catch(System.Exception e)
					{
						SqlDBObjects.SqlDbApplication.ShowErrorMessage(e);
						throw e;
					}
				}
			}
			return strResult;
		}
		public System.Object ConvertBack(System.Object value, System.Type targetType, System.Object parameter, System.String language)
		{
			return null;
		}
	};

	//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	[MarkupExtensionReturnType(ReturnType = typeof(System.String))]
	public class FileTextExtension : Windows.UI.Xaml.Markup.MarkupExtension
	{
		public System.String FilePath { get; set; }
		public FileTextExtension() : base()
		{
		}
		public FileTextExtension(System.String strFilePath) : base()
		{
			FilePath = strFilePath;
		}
		protected override System.Object ProvideValue()
		{
			System.String strFileContent = null;
			if(!System.String.IsNullOrEmpty(FilePath))
			{
				try
				{
					strFileContent = System.IO.File.ReadAllText(FilePath);
				}
				catch(System.Exception e)
				{
					SqlDBObjects.SqlDbApplication.ShowErrorMessage(e);
					throw e;
				}
			}
			return strFileContent;
		}
	};
	[MarkupExtensionReturnType(ReturnType = typeof(System.String))]
	public class FileText : SqlDBObjects.FileTextExtension
	{
		public FileText() : base() { }
		public FileText(System.String strFilePath) : base(strFilePath) { }
		protected override System.Object ProvideValue()
		{
			return base.ProvideValue();
		}
	};

	//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	
	[MarkupExtensionReturnType(ReturnType = typeof(System.String))]
	public class AncestorOfTypeExtension : Windows.UI.Xaml.Markup.MarkupExtension
	{
		public System.Object Source { get; set; }
		public System.Type AncestorType { get; set; }
		public AncestorOfTypeExtension() : base()
		{
		}
		public AncestorOfTypeExtension(System.Type typeAncestor) : base()
		{
			AncestorType = typeAncestor;
		}
		protected override System.Object ProvideValue() =>
			Source != null && AncestorType != null && Source is DependencyObject ?
				(Source as DependencyObject).FindAscendants().Where<DependencyObject>(curdpobj => AncestorType.IsAssignableFrom(curdpobj.GetType())) :
				null;
	};
	[MarkupExtensionReturnType(ReturnType = typeof(System.String))]
	public class AncestorOfType : SqlDBObjects.AncestorOfTypeExtension
	{
		public AncestorOfType() : base() { }
		public AncestorOfType(System.Type typeAncestor) : base(typeAncestor) { }
		protected override System.Object ProvideValue()
		{
			return base.ProvideValue();
		}
	};
	
	//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	
	[MarkupExtensionReturnType(ReturnType = typeof(System.String))]
	public class TemplatedParentExtension : Windows.UI.Xaml.Markup.MarkupExtension
	{
		public System.Object Source { get; set; }
		public TemplatedParentExtension() : base()
		{
		}
		public TemplatedParentExtension(System.Object source) : base()
		{
			Source = source;
		}
		protected override System.Object ProvideValue()
		{
			Windows.UI.Xaml.FrameworkElement templatedParent = null;
			if(Source != null)
			{
				Windows.UI.Xaml.FrameworkTemplate template = (Source as DependencyObject).FindAscendant<Windows.UI.Xaml.FrameworkTemplate>();
				if(template != null)
				{
					templatedParent = template.FindAscendant<Windows.UI.Xaml.FrameworkElement>();
				}
			}
			return templatedParent;
		}
	};
	[MarkupExtensionReturnType(ReturnType = typeof(System.String))]
	public class TemplatedParent : SqlDBObjects.AncestorOfTypeExtension
	{
		public TemplatedParent() : base() { }
		public TemplatedParent(System.Type typeAncestor) : base(typeAncestor) { }
		protected override System.Object ProvideValue()
		{
			return base.ProvideValue();
		}
	};
	
	//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	[MarkupExtensionReturnType(ReturnType = typeof(System.Object))]
	public class StaticResourceExtension : Windows.UI.Xaml.Markup.MarkupExtension
	{
		public Windows.UI.Xaml.FrameworkElement Source { get; set; }
		public System.Object ResourceKey { get; set; }
		public StaticResourceExtension() : base()
		{
		}
		public StaticResourceExtension(System.Object key) : base()
		{
			ResourceKey = key;
		}
		protected override System.Object ProvideValue()
		{
			System.Object resource = null;
			if(ResourceKey != null)
			{
				SqlDbContentControl contentControl = Window.Current.Content as SqlDbContentControl;
				if(Source != null && Source.Resources != null && Source.Resources.ContainsKey(ResourceKey))
				{
					resource = Source.Resources[ResourceKey];
				}
				else if(contentControl != null && contentControl.Resources != null && contentControl.Resources.ContainsKey(ResourceKey))
				{
					resource = contentControl.Resources[ResourceKey];
				}
				else if(Application.Current.Resources != null && Application.Current.Resources.ContainsKey(ResourceKey))
				{
					resource = Application.Current.Resources[ResourceKey];
				}
			}
			return resource;
		}
	};
	[MarkupExtensionReturnType(ReturnType = typeof(System.Object))]
	public class StaticResource : SqlDBObjects.StaticResourceExtension
	{
		public StaticResource() : base() { }
		public StaticResource(System.Object key) : base(key) { }
		protected override System.Object ProvideValue()
		{
			return base.ProvideValue();
		}
	};

	//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	[MarkupExtensionReturnType(ReturnType = typeof(System.Object))]
	public class ThemeResourceExtension : Windows.UI.Xaml.Markup.MarkupExtension
	{
		public System.Object ResourceKey { get; set; }
		public ThemeResourceExtension() : base()
		{
		}
		public ThemeResourceExtension(System.Object key) : base()
		{
			ResourceKey = key;
		}
		protected override System.Object ProvideValue()
		{
			System.Object resource = null;
			if(ResourceKey != null && Application.Current.Resources != null && Application.Current.Resources.ContainsKey(ResourceKey))
			{
				resource = Application.Current.Resources[ResourceKey];
			}
			return resource;
		}
	};
	[MarkupExtensionReturnType(ReturnType = typeof(System.Object))]
	public class ThemeResource : SqlDBObjects.ThemeResourceExtension
	{
		public ThemeResource() : base() { }
		public ThemeResource(System.Object key) : base(key) { }
		protected override System.Object ProvideValue()
		{
			return base.ProvideValue();
		}
	};

	//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------

	sealed partial class SqlDbApplication : Application
	{
		private static Windows.UI.Popups.MessageDialog MessageBox { get; set; } = new Windows.UI.Popups.MessageDialog("");
		public SqlDBObjects.SqlDbContentControl contentControl;
		public SqlDbApplication()
		{
			this.InitializeComponent();
			this.Suspending += OnSuspending;
		}
		protected override void OnLaunched(LaunchActivatedEventArgs e)
		{
            contentControl = (SqlDBObjects.SqlDbContentControl)Window.Current.Content;
            if(contentControl == null)
            {
                contentControl = new SqlDBObjects.SqlDbContentControl();
                Window.Current.Content = contentControl;
				Window.Current.Activate();
            }
 		}
		private void OnSuspending(object sender, SuspendingEventArgs e)
		{
			var deferral = e.SuspendingOperation.GetDeferral();
			deferral.Complete();
		}
		public static async System.Threading.Tasks.Task<Windows.UI.Xaml.Controls.ContentDialogResult> ShowContentDialogAsync(System.String strMessage)
		{
			return await ShowContentDialogAsync(strMessage, "OK", "Cancel");
		}
		public static async System.Threading.Tasks.Task<Windows.UI.Xaml.Controls.ContentDialogResult> ShowContentDialogAsync(System.String strMessage, System.String strPrimaryButtonText, System.String strCloseButtonText)
		{
			Windows.UI.Xaml.Controls.ContentDialog contentDialog = new Windows.UI.Xaml.Controls.ContentDialog();
			contentDialog.Title = strMessage;
			contentDialog.PrimaryButtonText = !System.String.IsNullOrEmpty(strPrimaryButtonText) ? strPrimaryButtonText : "OK";
			contentDialog.CloseButtonText = !System.String.IsNullOrEmpty(strCloseButtonText) ? strCloseButtonText : "Cancel";
			return await contentDialog.ShowAsync();
		}
		public static async void ShowMessage(System.String strMessage)
		{
			MessageBox.Content = strMessage;
			await MessageBox.ShowAsync();
		}
		public static async void ShowErrorMessage(System.Exception e)
		{
			MessageBox.Content = e.Message + "\nHResult: " + e.HResult.ToString() + "\nSource: " + e.Source + "\nStack trace: " + e.StackTrace;
			await MessageBox.ShowAsync();
		}
		public static void FillDialogControls(System.Collections.Generic.Dictionary<System.String, Windows.UI.Xaml.DependencyObject> dictControls, System.Object obj)
		{
			if(dictControls != null && obj != null)
			{
				Windows.UI.Xaml.FrameworkElement element = null;
				System.Collections.IEnumerable children = null;
				System.String strKey = null;
				if(typeof(Windows.UI.Xaml.Controls.UIElementCollection).IsAssignableFrom(obj.GetType()))
				{
					children = (System.Collections.IEnumerable)obj;
				}
				else if(typeof(Windows.UI.Xaml.FrameworkElement).IsAssignableFrom(obj.GetType()))
				{
					element = (Windows.UI.Xaml.FrameworkElement)obj;
					strKey = element.Name;
					if(!System.String.IsNullOrEmpty(strKey))
					{
						if(dictControls.ContainsKey(strKey))
						{
							dictControls[strKey] = element;
						}
						else
						{
							dictControls.Add(strKey, element);
						}
					}
					children = element.FindChildren();
					/*
					int nChildrenCount = Windows.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(element);
					if(nChildrenCount > 0)
					{
						children = new List<Windows.UI.Xaml.DependencyObject>();
						for(int idx = 0; idx < nChildrenCount; idx++)
						{
							(children as List<Windows.UI.Xaml.DependencyObject>).Add(Windows.UI.Xaml.Media.VisualTreeHelper.GetChild(element, idx));
						}
					}
					*/
				}
				if(children != null)
				{
					foreach(System.Object curChild in children)
					{
						FillDialogControls(dictControls, curChild);
					}
				}
			}
		}
		public static T FindVisualChildByType<T>(Windows.UI.Xaml.DependencyObject parent) where T : Windows.UI.Xaml.DependencyObject
		{
			T resChild = null;
			if(parent != null)
			{
				if(Windows.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(parent) == 0 && typeof(Windows.UI.Xaml.Controls.Control).IsAssignableFrom(parent.GetType()))
				{
					((Windows.UI.Xaml.Controls.Control)parent).ApplyTemplate();
				}
				resChild = System.Linq.Enumerable.First<T>(System.Linq.Enumerable.OfType<T>(parent.FindDescendants()));
			}
			return resChild;
		}
		public static T FindVisualChildByTypeAndName<T>(Windows.UI.Xaml.DependencyObject parent, System.String strName) where T : Windows.UI.Xaml.DependencyObject
		{
			T resChild = null;
			if(parent != null)
			{
				if(Windows.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(parent) == 0 && typeof(Windows.UI.Xaml.Controls.Control).IsAssignableFrom(parent.GetType()))
				{
					((Windows.UI.Xaml.Controls.Control)parent).ApplyTemplate();
				}
				foreach(Windows.UI.Xaml.DependencyObject curChild in System.Linq.Enumerable.OfType<T>(parent.FindDescendants()))
				{
					if(curChild != null && System.String.Compare(((Windows.UI.Xaml.FrameworkElement)curChild).Name, strName) == 0)
					{
						resChild = (T)curChild;
						break;
					}
				}
			}
			return resChild;
		}
		public static T FindVisualParentByType<T>(Windows.UI.Xaml.DependencyObject dpobj) where T : Windows.UI.Xaml.DependencyObject
		{
			T resParent = null;
			if(dpobj != null)
			{
				resParent = System.Linq.Enumerable.First<T>(System.Linq.Enumerable.OfType<T>(dpobj.FindAscendants()));
			}
			return resParent;
		}
		public static T FindVisualParentByTypeAndName<T>(Windows.UI.Xaml.DependencyObject dpobj, System.String strName) where T : Windows.UI.Xaml.DependencyObject
		{
			T resParent = null;
			if(dpobj != null)
			{
				foreach(Windows.UI.Xaml.DependencyObject curParent in System.Linq.Enumerable.OfType<T>(dpobj.FindAscendants()))
				{
					if(curParent != null && typeof(Windows.UI.Xaml.FrameworkElement).IsAssignableFrom(curParent.GetType())
						&& System.String.Compare(((Windows.UI.Xaml.FrameworkElement)curParent).Name, strName) == 0)
					{
						resParent = (T)dpobj;
						break;
					}
				}
			}
			return resParent;
		}
	}
}
