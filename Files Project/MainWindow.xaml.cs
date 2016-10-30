using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Files_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String filePath;
        private String fileName = "untitled.txt";
        private Boolean hasTextChanged;
        private Boolean isCloseButtonTriggered;
        Color selectedInitialColor;
        public MainWindow()
        {
            InitializeComponent();
            fontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            fontColor.ItemsSource = typeof(Colors).GetProperties();
            string richText = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;
            if (String.IsNullOrWhiteSpace(richText))
            {
                TextRange richTextEditorRange = new TextRange(richTextBox.Document.ContentEnd, richTextBox.Document.ContentEnd);
                richTextEditorRange.ApplyPropertyValue(Inline.FontFamilyProperty, fontFamily.SelectedItem);
                Color selectedColor = (Color)(fontColor.SelectedItem as PropertyInfo).GetValue(null, null);
                richTextEditorRange.ApplyPropertyValue(TextElement.ForegroundProperty, selectedColor.ToString());
            }
                this.Title = "Professional Editor "+"("+ fileName + ")";
                hasTextChanged = false;
                messageLabel.Content = "";
                Canvas canvas = (Canvas)messageLabel.Parent;
                Color color = (Color)ColorConverter.ConvertFromString("#FF6D8585");
                canvas.Background = new System.Windows.Media.SolidColorBrush(color);
            isCloseButtonTriggered = false;
        }

        private void MenuItem_Open(object sender, ExecutedRoutedEventArgs e)
        {
            if(hasTextChanged)
            {         
                    MessageBoxResult result = MessageBox.Show("Do you want to save changes to " + fileName, "Save File", MessageBoxButton.YesNoCancel);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            if (null != filePath)
                            {
                            saveFunction();
                            }
                            else
                            {
                                MenuItem_SaveAs(sender, e);
                            }
                            break;
                        case MessageBoxResult.No:
                        OpenFunction();
                        break;
                        case MessageBoxResult.Cancel:
                            break;
                    }
                }
            else
            {
                OpenFunction();
            }            
        }

        private void OpenFunction()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Text files (*.txt)|*.txt";
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    FileStream fileStream = new FileStream(dlg.FileName, FileMode.Open);
                    TextRange range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                    range.Load(fileStream, DataFormats.Text);
                    filePath = dlg.FileName;
                    fileName = dlg.SafeFileName;
                    fileStream.Close();
                }
                catch(Exception ex)
                {
                    messageLabel.Content = "An error occured while Opening a file "+ex.Message;
                }
                this.Title = "Professional Editor " + "(" + fileName + ")";
                hasTextChanged = false;                
                messageLabel.Content = "Opened Successfully";
                Canvas canvas = (Canvas)messageLabel.Parent;
                Color color = (Color)ColorConverter.ConvertFromString("#FF6D8585");
                canvas.Background = new System.Windows.Media.SolidColorBrush(color);
            }
        }

        private void MenuItem_Save(object sender, ExecutedRoutedEventArgs e)
        {
            if (null != filePath)
            {
                saveFunction();
            }
            else
            {
                MenuItem_SaveAs(sender, e);
            }

        }

        private void saveFunction()
        {
            try
            {
                FileStream fileStream = new FileStream(filePath, FileMode.Create);
                TextRange range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                range.Save(fileStream, DataFormats.Text);
                this.Title = "Professional Editor " + "(" + fileName + ")";
                fileStream.Close();
            }
            catch (Exception ex)
            {
                messageLabel.Content = "An error occured while Saving a file "+ex.Message;
            }
            hasTextChanged = false;
            messageLabel.Content = "Saved Successfully";
            Canvas canvas = (Canvas)messageLabel.Parent;
            canvas.Background = Brushes.Green;
        }

        private void MenuItem_SaveAs(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Text files (*.txt)|*.txt";
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create);
                    TextRange range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                    range.Save(fileStream, DataFormats.Text);
                    filePath = dlg.FileName;
                    fileName = dlg.SafeFileName;
                    fileStream.Close();
                }
                catch (Exception ex)
                {
                    messageLabel.Content = "An error occured while Saving a file "+ex.Message;
                }
                this.Title = "Professional Editor " + "(" + fileName + ")";                
                hasTextChanged = false;
                messageLabel.Content = "Saved Successfully";
                Canvas canvas = (Canvas)messageLabel.Parent;
                canvas.Background = Brushes.Green;
            }
        }

        private void MenuItem_New(object sender, ExecutedRoutedEventArgs e)
        {
            if (hasTextChanged)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save changes to " + fileName, "Save File", MessageBoxButton.YesNoCancel);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        if (null != filePath)
                        {
                            saveFunction();
                        }
                        else
                        {
                            MenuItem_SaveAs(sender, e);
                        }
                        break;
                    case MessageBoxResult.No:
                        newFunction();
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                }
            }
            else
            {
                newFunction();
            }
        }

        private void newFunction()
        {
            fileName = "untitled.txt";
            filePath = null;
            TextRange range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            range.Text = "";
            this.Title = "Professional Editor " + "(" + fileName + ")";
            hasTextChanged = false;
            messageLabel.Content = "";
            Canvas canvas = (Canvas)messageLabel.Parent;
            Color color = (Color)ColorConverter.ConvertFromString("#FF6D8585");
            canvas.Background = new System.Windows.Media.SolidColorBrush(color);
        }

        private void MenuItem_Delete(object sender, RoutedEventArgs e)
        {
            if(null != filePath)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to Delete this file " + fileName, "Delete File", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        deleteFunction();
                        break;
                    case MessageBoxResult.No:
                        break;
                }                
            }
            else
            {
                messageLabel.Content = "No file is open to delete";
                Canvas canvas = (Canvas)messageLabel.Parent;
                canvas.Background = Brushes.Red;
            }
        }

        private void deleteFunction()
        {
            try
            {
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                messageLabel.Content = "An error occured while Deleting a file "+ex.Message;
            }
            newFunction();
            messageLabel.Content = "Deleted Successfully";
            Canvas canvas = (Canvas)messageLabel.Parent;
            canvas.Background = Brushes.Green;
        }

        private void fontFamily_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (fontFamily.SelectedItem != null)
            {
                TextRange range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                range.ApplyPropertyValue(Inline.FontFamilyProperty, fontFamily.SelectedItem);
                richTextBox.Focus();
            }
        }

        private void fontColor_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (fontColor.SelectedItem != null)
            {
                Color selectedColor = (Color)(fontColor.SelectedItem as PropertyInfo).GetValue(null, null);
                selectedInitialColor = selectedColor;
                TextRange range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                range.ApplyPropertyValue(TextElement.ForegroundProperty, selectedColor.ToString());
                richTextBox.Focus();
            }
        }

        private void fontSize_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (fontSize.SelectedItem != null)
            {
                TextRange range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                range.ApplyPropertyValue(Inline.FontSizeProperty, fontSize.SelectedItem);
                richTextBox.Focus();
            }
        }

        private void MenuItem_Close(object sender, RoutedEventArgs e)
        {
            isCloseButtonTriggered = true;
            if (hasTextChanged)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save changes to " + fileName, "Save File", MessageBoxButton.YesNoCancel);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        if (null != filePath)
                        {
                            saveFunction();
                            if(messageLabel.Content.ToString() == "Saved Successfully")
                            {
                                Application.Current.Shutdown();
                            }
                            else
                            {
                                isCloseButtonTriggered = false;
                            }
                            
                        }
                        else
                        {
                            MenuItem_SaveAs(sender, e);
                            if (messageLabel.Content.ToString() == "Saved Successfully")
                            {
                                Application.Current.Shutdown();
                            }
                            else
                            {
                                isCloseButtonTriggered = false;
                            }
                        }
                        break;
                    case MessageBoxResult.No:
                        Application.Current.Shutdown();
                        break;
                    case MessageBoxResult.Cancel:
                        isCloseButtonTriggered = false;
                        break;
                }
            }
            else
            {
                Application.Current.Shutdown();
            }
        }



       private void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!isCloseButtonTriggered)
            {
                if (hasTextChanged)
                {
                    MessageBoxResult result = MessageBox.Show("Do you want to save changes to " + fileName, "Save File", MessageBoxButton.YesNoCancel);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            if (null != filePath)
                            {
                                saveFunction();
                                if (messageLabel.Content.ToString() == "Saved Successfully")
                                {
                                    Application.Current.Shutdown();
                                }
                                else
                                {
                                    e.Cancel = true;
                                }
                            }
                            else
                            {
                                MenuItem_SaveAs(sender, new RoutedEventArgs());
                                if (messageLabel.Content.ToString() == "Saved Successfully")
                                {
                                    Application.Current.Shutdown();
                                }
                                else
                                {
                                    e.Cancel = true;
                                }
                            }
                            break;
                        case MessageBoxResult.No:
                            Application.Current.Shutdown();
                            break;
                        case MessageBoxResult.Cancel:
                            e.Cancel = true;
                            break;
                    }
                }
                else
                {
                    Application.Current.Shutdown();
                }
            }
        }

    private void richTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            hasTextChanged = true;
            messageLabel.Content = "Text is being modified...";
            Canvas canvas = (Canvas) messageLabel.Parent;
            canvas.Background = Brushes.Orange;
            this.Title = "Professional Editor " + "(" + fileName + "*)";
            TextRange range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            try
            {
                if (null != fontFamily && null != fontFamily.SelectedItem)
                {
                    range.ApplyPropertyValue(TextElement.FontFamilyProperty, fontFamily.SelectedItem);
                }
                if (null != fontColor && null != fontColor.SelectedItem)
                {
                    try
                    {
                        Color selectedColor = (Color)(fontColor.SelectedItem as PropertyInfo).GetValue(null, null);
                        range.ApplyPropertyValue(TextElement.ForegroundProperty, selectedColor.ToString());
                    }
                    catch (Exception ex)
                    {
                        messageLabel.Content = "An error occured during application start up." + ex.Message;
                    }
                }
                if (null != fontSize && null != fontSize.SelectedItem)
                {
                    range.ApplyPropertyValue(TextElement.FontSizeProperty, fontSize.SelectedItem);
                }
            }
            catch (Exception ex)
            {
                messageLabel.Content = "An error occured during user input "+ex.Message;
            }
        }

        private void richTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
                TextRange range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                if (!string.IsNullOrWhiteSpace(range.Text) && range.Text.ToString().Length > 1)
                {
                object temp = richTextBox.Selection.GetPropertyValue(Inline.FontWeightProperty);
                btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
                if(btnBold.IsChecked==true && range.Text.ToString().Length == 3)
                {
                    TextRange initialText = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                    initialText.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
                }
                temp = richTextBox.Selection.GetPropertyValue(Inline.FontStyleProperty);
                btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
                temp = richTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
                btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));
                temp = richTextBox.Selection.GetPropertyValue(Inline.FontFamilyProperty);
                fontFamily.SelectedItem = temp;
                temp = richTextBox.Selection.GetPropertyValue(Inline.FontSizeProperty);
                fontSize.Text = temp.ToString();
                fontColor.SelectedItem = selectedInitialColor.ToString();
            }
        }

        private void MenuItem_Reset(object sender, RoutedEventArgs e)
        {
            fontFamily.SelectedItem = fontFamily.Items[0];
            fontSize.SelectedItem = fontSize.Items[4];
            fontColor.SelectedItem = fontColor.Items[7];
            btnBold.IsChecked = false;
            btnItalic.IsChecked = false;
            btnUnderline.IsChecked = false;
            btnAlignLeft.IsChecked = false;
            btnAlignRight.IsChecked = false;
            btnAlignCenter.IsChecked = false;
            try
            {
                TextRange range = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                range.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Regular);
                range.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Normal);
                range.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
            }
            catch (Exception ex)
            {
                messageLabel.Content = "An error occured during reset "+ex.Message;
            }
            richTextBox.Focus();
        }

        private void MenuItem_About(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Show();
        }
    }
}
