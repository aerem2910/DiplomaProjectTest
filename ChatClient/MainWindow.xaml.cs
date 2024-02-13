using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using ChatClient.ServiceChat;
using Microsoft.Win32;

namespace ChatClient
{
    public partial class MainWindow : Window, IServiceChatCallback
    {
        private bool isConnected = false;
        private ServiceChatClient client;
        private int ID;
        private OpenFileDialog fileDialog;
        private ListBox lbImages;
        private readonly List<byte[]> imageList = new List<byte[]>();

        public MainWindow()
        {
            InitializeComponent();
            fileDialog = new OpenFileDialog();
            lbImages = new ListBox();

           lbImages.SelectionChanged += LbImages_SelectionChanged;
        }


        private void ConnectUser()
        {
            try
            {
                if (!isConnected)
                {
                    client = new ServiceChatClient(new InstanceContext(this));
                    ID = client.Connect(tbUserName.Text);
                    tbUserName.IsEnabled = false;
                    bConnDicon.Content = "Disconnect";
                    isConnected = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting: {ex.Message}");
            }
        }

        private void DisconnectUser()
        {
            try
            {
                if (isConnected)
                {
                    client.Disconnect(ID);
                    client = null;
                    tbUserName.IsEnabled = true;
                    bConnDicon.Content = "Connect";
                    isConnected = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error disconnecting: {ex.Message}");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                DisconnectUser();
            }
            else
            {
                ConnectUser();
            }
        }

        public void MessageCallback(string message)
        {
            lbChat.Items.Add(message);
            lbChat.ScrollIntoView(lbChat.Items[lbChat.Items.Count - 1]);
        }

        public void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisconnectUser();
        }

        public void tbMessage_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == System.Windows.Input.Key.Enter)
                {
                    if (client != null)
                    {
                        client.SendMessage(tbMessage.Text, ID);
                        tbMessage.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending message: {ex.Message}");
            }
        }

        public void ImageCallback(ImageMessage imageMessage, int id, string senderName)
        {
            try
            {
                
                Dispatcher.Invoke(() =>
                {
                    lbImages.Items.Add($"{senderName}: {imageMessage.ImageName}");
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обработке обратного вызова изображения: {ex.Message}");
            }
        }


        public void ListboxSelected()
        {
            try
            {
                if (lbImages.SelectedIndex >= 0)
                {
                    // Получение байтового массива изображения по индексу из списка
                    byte[] imageData = client.GetImage(lbImages.SelectedIndex);

                    // Сохранение изображения в файл
                    string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ReceivedImage.jpg");
                    File.WriteAllBytes(savePath, imageData);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}");
            }
        }

        public void LbImages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListboxSelected();
        }

        private byte[] LoadImageToByteArray(string filePath)
        {
            try
            {
                return File.ReadAllBytes(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image to byte array: {ex.Message}");
                return null;
            }
        }


        private void btnSendImage_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                try
                {
                    if (fileDialog.ShowDialog() == true)
                    {
                        // Загрузка изображения в байтовый массив
                        byte[] imageData = LoadImageToByteArray(fileDialog.FileName);

                        if (imageData != null)
                        {
                            // Отправка изображения на сервер
                            client.SendImage(imageData, Path.GetFileName(fileDialog.FileName), ID);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error sending image: {ex.Message}");
                }
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }




    }



}

