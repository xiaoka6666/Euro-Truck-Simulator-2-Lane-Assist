using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ETS2LA.Settings.Global;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace ETS2LA.UI.Views.Settings;

public partial class AssistanceSettingsPage : UserControl, INotifyPropertyChanged
{
    public ObservableCollection<TabStripItemHandler> AccelerationOptions { get; } = new();
    public ObservableCollection<TabStripItemHandler> SteeringSensitivityOptions { get; } = new();
    public ObservableCollection<TabStripItemHandler> FollowingDistanceOptions { get; } = new();
    public ObservableCollection<TabStripItemHandler> SetSpeedBehaviourOptions { get; } = new();
    public ObservableCollection<TabStripItemHandler> SpeedLimitWarningOptions { get; } = new();
    public ObservableCollection<TabStripItemHandler> CollisionAvoidanceOptions { get; } = new();

    public bool SeparateCruiseAndSteering
    {
        get => AssistanceSettings.Current.SeparateCruiseAndSteering;
        set
        {
            if (AssistanceSettings.Current.SeparateCruiseAndSteering != value)
            {
                AssistanceSettings.Current.SeparateCruiseAndSteering = value;
                AssistanceSettings.Current.Save();
            }
        }
    }

    public int SelectedAccelerationOption { get; set; }
    public int SelectedSteeringSensitivityOption { get; set; }
    public int SelectedFollowingDistanceOption { get; set; }
    public int SelectedSetSpeedBehaviourOption { get; set; }
    public int SelectedSpeedLimitWarningOption { get; set; }
    public int SelectedCollisionAvoidanceOption { get; set; }

    public AssistanceSettingsPage()
    {
        LoadAccelerationOptions();
        LoadSteeringSensitivityOptions();
        LoadFollowingDistanceOptions();
        LoadSetSpeedBehaviourOptions();
        LoadSpeedLimitWarningOptions();
        LoadCollisionAvoidanceOptions();
        AvaloniaXamlLoader.Load(this);
        DataContext = this;
    }

    private void LoadAccelerationOptions()
    {
        SelectedAccelerationOption = (int)AssistanceSettings.Current.AccelerationResponse;
        foreach (AccelerationResponseOption option in Enum.GetValues(typeof(AccelerationResponseOption)))
        {
            AccelerationOptions.Add(new TabStripItemHandler(option.ToString()));
        }
    }

    private void LoadSteeringSensitivityOptions()
    {
        SelectedSteeringSensitivityOption = (int)AssistanceSettings.Current.SteeringSensitivity;
        foreach (SteeringSensitivityOption option in Enum.GetValues(typeof(SteeringSensitivityOption)))
        {
            SteeringSensitivityOptions.Add(new TabStripItemHandler(option.ToString()));
        }
    }

    private void LoadFollowingDistanceOptions()
    {
        SelectedFollowingDistanceOption = (int)AssistanceSettings.Current.FollowingDistance;
        foreach (FollowingDistanceOption option in Enum.GetValues(typeof(FollowingDistanceOption)))
        {
            FollowingDistanceOptions.Add(new TabStripItemHandler(option.ToString()));
        }
    }

    private void LoadSetSpeedBehaviourOptions()
    {
        SelectedSetSpeedBehaviourOption = (int)AssistanceSettings.Current.SetSpeedBehaviourOption;
        foreach (SetSpeedBehaviour option in Enum.GetValues(typeof(SetSpeedBehaviour)))
        {
            SetSpeedBehaviourOptions.Add(new TabStripItemHandler(option.ToString()));
        }
    }

    private void LoadSpeedLimitWarningOptions()
    {
        SelectedSpeedLimitWarningOption = (int)AssistanceSettings.Current.SpeedLimitWarningOption;
        foreach (SpeedLimitWarning option in Enum.GetValues(typeof(SpeedLimitWarning)))
        {
            SpeedLimitWarningOptions.Add(new TabStripItemHandler(option.ToString()));
        }
    }

    private void LoadCollisionAvoidanceOptions()
    {
        SelectedCollisionAvoidanceOption = (int)AssistanceSettings.Current.CollisionAvoidanceOption;
        foreach (CollisionAvoidance option in Enum.GetValues(typeof(CollisionAvoidance)))
        {
            CollisionAvoidanceOptions.Add(new TabStripItemHandler(option.ToString()));
        }
    }

    private void OnAccelerationOptionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (SelectedAccelerationOption >= 0)
        {
            AssistanceSettings.Current.AccelerationResponse = (AccelerationResponseOption)SelectedAccelerationOption;
            AssistanceSettings.Current.Save();
        }
    }

    private void OnSteeringSensitivityChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (SelectedSteeringSensitivityOption >= 0)
        {
            AssistanceSettings.Current.SteeringSensitivity = (SteeringSensitivityOption)SelectedSteeringSensitivityOption;
            AssistanceSettings.Current.Save();
        }
    }

    private void OnFollowingDistanceChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (SelectedFollowingDistanceOption >= 0)
        {
            AssistanceSettings.Current.FollowingDistance = (FollowingDistanceOption)SelectedFollowingDistanceOption;
            AssistanceSettings.Current.Save();
        }
    }

    private void OnSetSpeedBehaviourChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (SelectedSetSpeedBehaviourOption >= 0)
        {
            AssistanceSettings.Current.SetSpeedBehaviourOption = (SetSpeedBehaviour)SelectedSetSpeedBehaviourOption;
            AssistanceSettings.Current.Save();
        }
    }

    private void OnSpeedLimitWarningChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (SelectedSpeedLimitWarningOption >= 0)
        {
            AssistanceSettings.Current.SpeedLimitWarningOption = (SpeedLimitWarning)SelectedSpeedLimitWarningOption;
            AssistanceSettings.Current.Save();
        }
    }

    private void OnCollisionAvoidanceChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (SelectedCollisionAvoidanceOption >= 0)
        {
            AssistanceSettings.Current.CollisionAvoidanceOption = (CollisionAvoidance)SelectedCollisionAvoidanceOption;
            AssistanceSettings.Current.Save();
        }
    }

    private void OnSeparateCruiseAndSteeringClicked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        SeparateCruiseAndSteering = !SeparateCruiseAndSteering;
        OnPropertyChanged(nameof(SeparateCruiseAndSteering));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class TabStripItemHandler: INotifyPropertyChanged
{
    public string Item { get; }
    public string Header => GetFormattedName();

    public TabStripItemHandler(string option)
    {
        Item = option;
    }

    private string GetFormattedName()
    {
        // Translate option names to Chinese
        return Item switch
        {
            // AccelerationResponseOption
            "Low" => "低",
            "Medium" => "中",
            "High" => "高",
            // SteeringSensitivityOption
            "Slow" => "慢",
            "Normal" => "正常",
            "Fast" => "快",
            // FollowingDistanceOption
            "Near" => "近",
            "Far" => "远",
            // SetSpeedBehaviour
            "SpeedLimit" => "限速",
            "CurrentSpeed" => "当前速度",
            // SpeedLimitWarning
            "Off" => "关闭",
            "Visual" => "视觉",
            "Chime" => "提示音",
            // CollisionAvoidance
            "Late" => "晚",
            "Early" => "早",
            _ => Item
        };
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}