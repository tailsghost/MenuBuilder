namespace MenuBuilder.Abstraction.Model;

public class NodeTypes
{
    public string NodeViewModelBase { get; init; }
    public string NodeViewModel { get; init; }
    public string MathOperationsNodeViewModel { get; init; }
    public string BitOperationsViewModel { get; init; }
    public string BitOperationNotViewModel { get; init; }
    public string ConnectionNodeViewModel { get; init; }
    public string TimerViewModel { get; init; }

    public NodeTypes()
    {
        NodeViewModelBase = "FBDEditor.Abstractions.ViewModels.NodeViewModelBase,FBDEditor";
        NodeViewModel = "FBDEditor.ViewModels.Nodes.NodeViewModel,FBDEditor";
        BitOperationsViewModel = "FBDEditor.ViewModels.Nodes.BitOperationsViewModel,FBDEditor";
        BitOperationNotViewModel = "FBDEditor.ViewModels.Nodes.BitOperationNotViewModel,FBDEditor";
        ConnectionNodeViewModel = "FBDEditor.ViewModels.Nodes.ConnectionNodeViewModel,FBDEditor";
        TimerViewModel = "FBDEditor.ViewModels.Nodes.TimerViewModel,FBDEditor";
        MathOperationsNodeViewModel = "FBDEditor.ViewModels.Nodes.MathOperationViewModel,FBDEditor";
    }
}
