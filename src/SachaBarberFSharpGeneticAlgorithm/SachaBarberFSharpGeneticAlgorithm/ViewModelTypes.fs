namespace ViewModelTypes

open System
open System.Windows
open FSharp.ViewModule
open FSharp.ViewModule.Validation
open FsXaml
open FsIOCWindow.ContainerTypes
open FsIOCWindow.ServiceTypes
open System.Windows.Input
open System.ComponentModel
open Microsoft.FSharp.Quotations.Patterns
open System.ComponentModel
open log4net
open System.Reactive.Disposables

//Simple base class for VMs that provides INPC
//functionality
//http://www.fssnip.net/2x
type ObservableObject () =
    let propertyChanged = 
        Event<PropertyChangedEventHandler,PropertyChangedEventArgs>()
    let getPropertyName = function 
        | PropertyGet(_,pi,_) -> pi.Name
        | _ -> invalidOp "Expecting property getter expression"
    interface INotifyPropertyChanged with
        [<CLIEvent>]
        member this.PropertyChanged = propertyChanged.Publish
    member this.NotifyPropertyChanged propertyName = 
        propertyChanged.Trigger(this,PropertyChangedEventArgs(propertyName))
    member this.NotifyPropertyChanged quotation = 
        quotation |> getPropertyName |> this.NotifyPropertyChanged

//Simple RelayCommand (ALA Josh Smith, to allow ICommands inside VM)
//http://sergeytihon.wordpress.com/2013/04/27/wpf-mvvm-with-xaml-type-provider/
type RelayCommand (canExecute:(obj -> bool), action:(obj -> unit)) =
    let event = new DelegateEvent<EventHandler>()
    interface ICommand with
        [<CLIEvent>]
        member x.CanExecuteChanged = event.Publish
        member x.CanExecute arg = canExecute(arg)
        member x.Execute arg = action(arg)




// MainWindowViewModel
type MainWindowViewModel(_populationInitialiser:IPopulationInitialiser) =
    inherit ObservableObject()
    let populationInitialiser = _populationInitialiser
    static let log = LogManager.GetLogger(Operators.typeof<MainWindowViewModel>)

    let mutable message = ""
    let disposables = new CompositeDisposable()
    do 
        disposables.Add(populationInitialiser.InitialPopulationStream
            .Subscribe(fun population -> 
                log.Info("Setting initial population from populationInitialiser")
                message <- population
            ))
        log.Info("Constructed MainWindowViewModel")
    
    member this.Message
        with get () = message
        and set value = 
            if message <> value then
                message <- value
                this.NotifyPropertyChanged <@ this.Message @>

    member this.OkCommand = 
        new RelayCommand ((fun canExecute -> true), 
            (fun action -> MessageBox.Show(this.Message) |> ignore))

    member this.WindowClosingCommand = 
        new RelayCommand ((fun canExecute -> true), 
            (fun action -> MessageBox.Show("Closing") |> ignore))
    

//Some useful attached DPs    
[<AbstractClass; Sealed>]
type AttachedProps private () =

   static let log = LogManager.GetLogger(Operators.typeof<MainWindowViewModel>)

   // Register the attached property, with some metadata that will allow a callback
   static let metadata = 
        new PropertyMetadata
            (null, new PropertyChangedCallback
                (fun dpo args ->
                    (
                        if args.NewValue <> null then
                            AttachedProps.OnViewModelTypeChanged(
                                dpo :?> Window,  args.NewValue :?> System.Type)
                    )
                )                 
            )

   static member private OnViewModelTypeChanged(window:Window, vmType:System.Type) =
        log.Debug(String.Format("ViewModelType was : {0}", vmType))
        let container = IOCManager.Instance.Container
        let genericResolveMethod = 
            container.GetType().GetMethods() 
            |> Seq.filter(fun meth -> meth.Name.Equals("Resolve")) 
            |> Seq.head

        let concreteResolveMethod =
            genericResolveMethod.MakeGenericMethod [| vmType |]           
        let vm = concreteResolveMethod.Invoke (container, Array.empty)
        window.DataContext <- vm
        ()


   static member ViewModelTypeProperty = 
       DependencyProperty.RegisterAttached("ViewModelType", 
            typeof<System.Type>, typeof<AttachedProps>,metadata) 
   
   // Set the property
   static member SetViewModelType (element:UIElement, value:System.Type) =
       let typeValue = value
       element.SetValue(AttachedProps.ViewModelTypeProperty, typeValue)
   
   // Get the property
   static member GetViewModelType (element:UIElement) = 
       element.GetValue AttachedProps.ViewModelTypeProperty       