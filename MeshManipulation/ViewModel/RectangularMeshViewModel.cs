using Common;
using Mesh;
using Mesh.Creation;
using Mesh.Division;
using Mesh.FiniteElement;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace MeshManipulation.ViewModel
{
    public class RectangularMeshViewModel : INotifyPropertyChanged
    {
        #region CONSTRUCTORS

        public RectangularMeshViewModel()
        {
            RegisterCommands();
        }

        #endregion

        #region INITIALIZATION

        protected void RegisterCommands()
        {
            CreateMeshCommand = new DelegateCommand(CreateMeshCommand_Execute);
            RemoveMeshCommand = new DelegateCommand(RemoveMeshCommand_Execute);
            PutPointCommand = new DelegateCommand<object>(PutPointCommand_Execute, PutPointCommand_CanExecute);
            DivideMeshCommand = new DelegateCommand<object>(DivideMeshCommand_Execute);
        }

        #endregion

        #region FIELDS

        #endregion

        #region PROPERTIES

            #region InitialVertices

            ObservableCollection<Point> _initialVertices = new ObservableCollection<Point>();
            /// <summary>
            /// punkt poczatkowy dla siatki
            /// </summary>
            public ObservableCollection<Point> InitialVertices
            {
                get
                {
                    if (MeshCreator == null) return null;

                    return new ObservableCollection<Point>(
                        (MeshCreator as RectMeshFromTwoPointsCreator).InitialPoints);
                }
            }

            #endregion

            #region Mesh
            private RectangularMesh _mesh = null;
            /// <summary>
            /// siatka elementow skonczonych
            /// </summary>
            public RectangularMesh Mesh
            {
                get { return _mesh; }
                set
                {
                    _mesh = value;
                    RaisePropertyChanged();
                    RaisePropertyChanged("Elements");
                    RaisePropertyChanged("Vertices");
                }
            } 
            #endregion

            #region MeshCreator
            /// <summary>
            /// tworca siatki
            /// </summary>
            public IMeshCreator MeshCreator { get; set; }

            #endregion

            #region Elements
            /// <summary>
            /// kolekcja elementow siatki - do bindowania
            /// </summary>
            public ObservableCollection<AbstractElement> Elements
            {
                get
                {
                    if (Mesh == null)
                    {
                        return new ObservableCollection<AbstractElement>();
                    }
                    return new ObservableCollection<AbstractElement>(Mesh.Elements);
                }
            } 
            #endregion

            #region Vertices
            /// <summary>
            /// kolekcja wierzcholkow elementow siatki
            /// </summary>
            public ObservableCollection<Point> Vertices
            {
                get
                {
                    if (Mesh == null || Mesh.Elements == null)
                        return new ObservableCollection<Point>();

                    var vertices = from element in Mesh.Elements
                                   from point in element.Points
                                   select point;

                    return new ObservableCollection<Point>(vertices.Distinct());
                }
            } 
            #endregion

            #region DivisionPercent
            private int _divisionPercent = 0;
            /// <summary>
            /// Ile procent elementow zostalo podzielonych
            /// </summary>
            public int DivisionPercent
            {
                get { return _divisionPercent; }
                set { _divisionPercent = value; RaisePropertyChanged("DivisionPercent"); }
            } 
            #endregion

        #endregion

        #region COMMANDS

            #region CreateMeshCommand

            /// <summary>
            /// komenda rozpoczynajaca proces tworzenia siatki
            /// </summary>
            public ICommand CreateMeshCommand { get; set; }
            private void CreateMeshCommand_Execute()
            {
                Mesh = null;
                MeshCreator = new RectMeshFromTwoPointsCreator();
            } 

            #endregion

            #region RemoveMeshCommand

            /// <summary>
            /// komenda przerywa proces tworzenia siatki i czysci siatke
            /// </summary>
            public ICommand RemoveMeshCommand { get; set; }
            private void RemoveMeshCommand_Execute()
            {
                Mesh = null;
                MeshCreator = null;
            }

            #endregion

            #region PutPointCommand

            /// <summary>
            /// wstawienie punktu jako wierzcholka siatki (uzywane do utworzenia glownego prostokata siatki)
            /// </summary>
            public ICommand PutPointCommand { get; set; }
            private void PutPointCommand_Execute(object meshArea)
            {
                // jesli kreator nie ma siatki, wyjdz z funkcji
                if (MeshCreator == null || MeshCreator.Mesh != null)
                {
                    return;
                }

                // stworz punkt z pozycji myszki
                System.Windows.Controls.Grid area = meshArea as System.Windows.Controls.Grid;
                System.Windows.Point mousePosition = Mouse.GetPosition(area);
                Point p = new Point(mousePosition.X, mousePosition.Y);

                // dodaj go do kreatora i jesli wygeneruje on siatke, to ja przypisz sobie
                if (MeshCreator.PutPoint(p))
                {
                    Mesh = MeshCreator.Mesh as RectangularMesh;
                    MeshCreator = null;
                }
                RaisePropertyChanged("InitialVertices");
            }
            
            /// <summary>
            /// czy mozna wstawic punkt
            /// </summary>
            /// <returns></returns>
            private bool PutPointCommand_CanExecute(object parameter)
            {
                if (MeshCreator == null)
                {
                    return false;
                }
                else if (MeshCreator.Mesh == null)
                {
                    return true;
                }

                return false;
            }

            #endregion

            #region DivideMeshCommand

            /// <summary>
            /// komenda dzielaca siatke
            /// </summary>
            public ICommand DivideMeshCommand { get; set; }
            private void DivideMeshCommand_Execute(object progressBar)
            {
                if (Mesh != null)
                {

                    TaskScheduler uiThread = TaskScheduler.FromCurrentSynchronizationContext();
                    Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
                    System.Windows.Controls.ProgressBar divisionProgressBar =
                        progressBar as System.Windows.Controls.ProgressBar;

                    Task DividerTask = Task<List<AbstractElement>>.Factory.StartNew(() => {
                        IMeshDivider divider = new SimpleMeshDivider(0.01);

                        divider.PercentCompleted += (percent) =>
                        {
                            dispatcher.Invoke(() =>
                            {
                                int newPercent = (int)Math.Floor(percent * 100.0);
                                //DivisionPercent = newPercent;
                                divisionProgressBar.Value = newPercent;
                            });
                        };

                        List<AbstractElement> newElements = divider.DivideMesh(Mesh.Elements);

                        dispatcher.Invoke(() =>
                        {
                            divisionProgressBar.Value = 100.0;
                        });

                        return newElements;
                    })
                    .ContinueWith((task) =>
                    {
                        Mesh.Elements = task.Result;
                        RaisePropertyChanged("Elements");
                        RaisePropertyChanged("Vertices");
                    }, uiThread);
                    
                }
            }

            #endregion

        #endregion
        
        #region INotifyPropertyChanged IMPLEMENTATION

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
