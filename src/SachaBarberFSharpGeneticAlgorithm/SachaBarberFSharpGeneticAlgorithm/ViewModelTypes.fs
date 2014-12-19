namespace ViewModelTypes

open System
open System.Windows
open FSharp.ViewModule
open FSharp.ViewModule.Validation
open FsXaml
open System.Windows.Input
open System.ComponentModel
open Microsoft.FSharp.Quotations.Patterns
open System.ComponentModel
open System.Reactive.Disposables
open System.Reactive.Concurrency
open System.Reactive.Linq
open System.Collections.Generic

open log4net

open FsIOCWindow.ContainerTypes
open FsIOCWindow.ServiceTypes
open FsIOCWindow.ModelTypes
open BioCSharp.Interfaces
open BioCSharp.Biomorphs





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



//Simple RelayCommand (ALA Josh Smith/Marlon Grech/Me any of the Wpf Disciples actually.
//This allows ICommands to run with their logic inside VM, by way of 
//Action<object> for ICommand.Execute / Predicate<object> for ICommand.CanExecute
//http://sergeytihon.wordpress.com/2013/04/27/wpf-mvvm-with-xaml-type-provider/
type RelayCommand (canExecute:(obj -> bool), action:(obj -> unit)) =
    let event = new DelegateEvent<EventHandler>()
    interface ICommand with
        [<CLIEvent>]
        member x.CanExecuteChanged = event.Publish
        member x.CanExecute arg = canExecute(arg)
        member x.Execute arg = action(arg)



// MainWindowViewModel
type MainWindowViewModel(_populationInitialiser:IPopulationInitialiser) as x =
    inherit ObservableObject()

    let populationInitialiser = _populationInitialiser
    let disposables = new CompositeDisposable()
    let mutable population = new List<EvolvableSkullViewModel>()

    static let log = LogManager.GetLogger(Operators.typeof<MainWindowViewModel>)

    do 
        disposables.Add(populationInitialiser.InitialPopulationStream
            .Subscribe(fun initialGeneSequencesFromCsv -> 
                let popCount = initialGeneSequencesFromCsv |> Seq.length
                log.Info(
                    String.Format(
                        "Setting initial Gene Sequences of {0} organisms from populationInitialiser", 
                        popCount))

                let initialGeneSequences = List.ofSeq(initialGeneSequencesFromCsv)

                let biomorphs = List.ofSeq (seq { for i in 0.. 8 
                    do yield new EvolvableSkullViewModel(x, Guid.NewGuid()) })

                for i = 0 to 8 do
                    biomorphs.[i].CreateGenes(initialGeneSequences.[i]);
                    (biomorphs.[i] :> IEvolvableSkull).ScoreOrganism()

                population <-  new List<EvolvableSkullViewModel>(biomorphs)
                x.StartSelectionProcessTimer()
            ))

        log.Info("Constructed MainWindowViewModel")



    interface IMainWindowViewModel  with


        member x.SortPopulationByFitness() = 

            //get the fitest organism, and use that as the dominant one that will
            //be used to make children from
            let comparer = new EvolvableSkullComparer();

            //don't disturb current Population, work with a copy of it
            let copyOfPopulation = new List<EvolvableSkullViewModel>(x.Population)
            copyOfPopulation.Sort(comparer)

            copyOfPopulation.Reverse()
            for i = 0 to 8 do
                if (x.Population.[i] :> IEvolvableSkull).Id = (copyOfPopulation.[0] :> IEvolvableSkull).Id then
                    x.Population.[i].IsSelected <- true
                else
                    x.Population.[i].IsSelected <- false
            (x :> IMainWindowViewModel).NewPopulationFromDominant(copyOfPopulation.[0])
            ()


        member x.NewPopulationFromDominant(chosenDominantOrganism:IEvolvableSkull) =  
            let clone =  chosenDominantOrganism.Clone()
            for i = 0 to 8 do
                if (i = x.Population.Count-1) then 
                    x.Population.[i].SetToOther(x.Population.[i - 1].MakeChild(clone))
                else
                    x.Population.[i].SetToOther(x.Population.[i + 1].MakeChild(clone))
            let copyOfPopulation = new List<EvolvableSkullViewModel>(x.Population)

            //assign new Population will force WPF UI to redraw population
            x.Population <- copyOfPopulation
            ()

    member this.Population
        with get () = population
        and set value = 
                population <- value
                this.NotifyPropertyChanged <@ this.Population @>
    
    member this.StartSelectionProcessTimer() =
        disposables.Add(Observable.Interval(TimeSpan.FromSeconds(0.3), Scheduler.TaskPool)
            .Subscribe(fun timer -> 
                async { 
                    try
                        for i = 0 to 8 do
                            let organism = x.Population.[i] 
                            (organism :> IEvolvableSkull).ScoreOrganism()
                        (x :> IMainWindowViewModel).SortPopulationByFitness()
                    with
                        | Failure(msg) -> 
                            log.Error("Error starting up, this could be caused by by AppSettings value for 'InitialPopulationLoadingStrategy'")
                } |> Async.RunSynchronously

            ))
        ()
    
    member this.OkCommand = 
        new RelayCommand ((fun canExecute -> true), 
            (fun action -> MessageBox.Show("This should show a F# Chart of current population") |> ignore))

    member this.WindowClosingCommand = 
        new RelayCommand ((fun canExecute -> true), 
            (fun action -> disposables.Dispose()))
    


    


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
       
              