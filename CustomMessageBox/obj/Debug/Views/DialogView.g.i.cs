// Updated by XamlIntelliSenseFileGenerator 1/17/2021 7:32:54 PM
#pragma checksum "..\..\..\Views\DialogView.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "E45382D61CB07ED6335AA94D31A921308E87C717241DCEFAE4CB74FBB7CD27C5"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using CustomMessageBox;
using CustomMessageBox.Behaviours;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace CustomMessageBox
{


    /// <summary>
    /// DialogModel
    /// </summary>
    public partial class DialogView : System.Windows.Window, System.Windows.Markup.IComponentConnector
    {


#line 21 "..\..\..\Views\DialogView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border NodeSelectionBorder;

#line default
#line hidden


#line 29 "..\..\..\Views\DialogView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid gridTop;

#line default
#line hidden


#line 35 "..\..\..\Views\DialogView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas CanvasNodeBaseLayerOne;

#line default
#line hidden


#line 36 "..\..\..\Views\DialogView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border border1;

#line default
#line hidden


#line 41 "..\..\..\Views\DialogView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Path Line;

#line default
#line hidden


#line 43 "..\..\..\Views\DialogView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock CaptionTxtBlock;

#line default
#line hidden


#line 44 "..\..\..\Views\DialogView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CLOSE;

#line default
#line hidden


#line 63 "..\..\..\Views\DialogView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image MessageBoxIcon;

#line default
#line hidden


#line 64 "..\..\..\Views\DialogView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock MessageBoxTextBlck;

#line default
#line hidden


#line 74 "..\..\..\Views\DialogView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ColumnDefinition MessageCancelGrid;

#line default
#line hidden


#line 77 "..\..\..\Views\DialogView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button MessageOk;

#line default
#line hidden


#line 84 "..\..\..\Views\DialogView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button MessageCancel;

#line default
#line hidden


#line 91 "..\..\..\Views\DialogView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button MessageYes;

#line default
#line hidden

        private bool _contentLoaded;

        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/CustomMessageBox;component/views/dialogview.xaml", System.UriKind.Relative);

#line 1 "..\..\..\Views\DialogView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);

#line default
#line hidden
        }

        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:

#line 10 "..\..\..\Views\DialogView.xaml"
                    ((CustomMessageBox.DialogModel)(target)).MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.Window_MouseDown);

#line default
#line hidden
                    return;
                case 2:
                    this.NodeSelectionBorder = ((System.Windows.Controls.Border)(target));
                    return;
                case 3:
                    this.gridTop = ((System.Windows.Controls.Grid)(target));
                    return;
                case 4:
                    this.CanvasNodeBaseLayerOne = ((System.Windows.Controls.Canvas)(target));
                    return;
                case 5:
                    this.border1 = ((System.Windows.Controls.Border)(target));
                    return;
                case 6:
                    this.Line = ((System.Windows.Shapes.Path)(target));
                    return;
                case 7:
                    this.CaptionTxtBlock = ((System.Windows.Controls.TextBlock)(target));
                    return;
                case 8:
                    this.CLOSE = ((System.Windows.Controls.Button)(target));
                    return;
                case 9:
                    this.MessageBoxIcon = ((System.Windows.Controls.Image)(target));
                    return;
                case 10:
                    this.MessageBoxTextBlck = ((System.Windows.Controls.TextBlock)(target));
                    return;
                case 11:
                    this.MessageCancelGrid = ((System.Windows.Controls.ColumnDefinition)(target));
                    return;
                case 12:
                    this.MessageOk = ((System.Windows.Controls.Button)(target));
                    return;
                case 13:
                    this.MessageCancel = ((System.Windows.Controls.Button)(target));
                    return;
                case 14:
                    this.MessageYes = ((System.Windows.Controls.Button)(target));
                    return;
            }
            this._contentLoaded = true;
        }
    }
}
