using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Drawing;

namespace Sample
{
    class Sample
    {
        static string[] lines;
        static void Main()
        {
            //bool isAchieved = false;
            Point coin = new Point(0, 0);
            Contestant p1 = new Contestant(0, new Point(0, 0));;
            lines = System.IO.File.ReadAllLines(@"C:\Users\dell\Documents\Visual Studio 2010\Projects\Console\Console\input.txt");
            foreach (string line in lines)
            {
                Console.WriteLine(line);
            }
            for (int i = 0; i < lines.Length; i++)
            {
                if(lines[i].Contains('c'))
                {
                    int y = i;
                    int x = lines[i].IndexOf('c');
                    coin = new Point(x, y);
                }
                if (lines[i].Contains('0'))
                {
                    int y = i;
                    int x = lines[i].IndexOf('0');
                    p1 = new Contestant(0, new Point(x, y));
                }
            }
            new Sample().AStar(p1.CurrentP,coin);
            Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();
        }

        #region A*
        public void AStar(Point source, Point dest)
        {
            List<Node> open = new List<Node>();
            List<Node> close = new List<Node>();
            open.Add(new Node(source));
            Node current;
            while(open.Count!=0)
            {
                current = open.ElementAt(0);
                if(current.P!=source)
                    current.Dist = current.Parent.Dist + 1;
                if (current.P == dest)
                {
                    Console.WriteLine("reached "+ current.Dist);
                    Node temp = current;
                    while (temp.Parent.P.X != -1)
                    {
                        Console.WriteLine(temp.P.X + " " + temp.P.Y);
                        temp = temp.Parent;
                    }
                    break;
                }
                #region forloop
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == 0 && j == 0)
                        {
                            continue;
                        }
                        if (i == 0 || j == 0)
                        {
                            Point p =new Point(current.P.X+i,current.P.Y+j);
                            Node temp = new Node(p, current, 0);
                            if (!close.Contains(temp) && reachable(p))
                            {
                                if (open.Contains(temp))
                                {
                                    if (open.ElementAt(open.IndexOf(temp)).Dist > current.Dist + heuristic(p, dest))
                                    {
                                        int hDist = current.Dist + heuristic(p, dest);
                                        temp = new Node(p, current, hDist);
                                    }
                                }
                                else
                                {
                                    int hDist = current.Dist + heuristic(p, dest);
                                    temp = new Node(p, current, hDist);
                                    open.Add(temp);
                                }

                            }

                        }
                    }
                }
                #endregion
                close.Add(current);
                open.Remove(current);
                open.OrderBy(x => x.HDist);
                //Console.WriteLine("working");              
            }
        }
        #endregion
        public bool reachable(Point p)
        {
            bool reachable=true;
            if (p.X < 0 || p.X > 9 || p.Y < 0 || p.Y > 9)
            {
                reachable=false;
            }
            else if(lines[p.Y].ElementAt(p.X)=='b'){
                //Console.WriteLine("working");
                reachable = false;
            }
            return reachable;
        }
        public int heuristic(Point p, Point dest) 
        {
            int h=0;
            h += Math.Abs(dest.X-p.X) + Math.Abs(dest.Y-p.Y);    
            return h;
        }
    }
    class Node
    {
        Point p;
        Node parent;
        int dist;
        int hDist;
        #region construct
        public Node(Point p, Node parent, int hDist)
        {
            this.p = p;
            this.parent = parent;
            this.hDist = hDist;
        }
        public Node(Point p)
        {
            this.p = p;
            this.parent = new Node();
            dist = 0;
        }
        public Node()
        {
            this.p = new Point(-1, -1);
        }
        #endregion

        public override bool Equals(Object obj)
        {
            Node n= (Node) obj;
            return p.Equals(n.p);
        }

        #region "properties"
        public Point P
        {
            get { return p; }
        }
        public Node Parent
        {
            get { return parent; }
        }
        public int Dist
        {
            get { return dist; }
            set { dist = value; }
        }
        public int HDist 
        {
            get { return hDist; }
        }
        #endregion
    }

}
