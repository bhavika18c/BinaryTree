using System;
using System.Collections.Generic;

namespace BinaryTree {

    public static class BinaryTreeParentFinder
    {      
        public class Node
        {
            public int data;
            public Node left, right;

            public Node(int item)
            {
                data = item;
                left = right = null;
            }
        }
        public class BinaryTree
        {
            private Node root;
        

            public static void Main(string[] args)
            {
                BinaryTree tree = new BinaryTree();

                //Construct The Tree
                tree.root = new Node(100);
                tree.root.left = new Node(4);
                tree.root.right = new Node(1);
                tree.root.right.left = new Node(5);
                tree.root.right.right = new Node(6);
                tree.root.right.right.right = new Node(12);
                tree.root.right.right.left = new Node(11);

                tree.root.Print();

                Console.WriteLine("Enter Node1 : ");
                int n1 = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Enter Node2 : ");
                int n2 = Convert.ToInt32(Console.ReadLine());

                Node bTree = tree.ParentFinder(tree.root, n1, n2);

                if (bTree != null)
                    Console.WriteLine("Parent of " + n1 + " and " + n2 + " is: " + bTree.data);

                else
                    Console.WriteLine("Value(s) entered is not present in Binary Tree.");
                Console.ReadKey();
             }

            public Node ParentFinder(Node node, int n1, int n2)
            {
                if (node == null)
                {
                    return null;
                }

                if (node.data == n1 || node.data == n2)
                {
                    return node;
                }

                Node left_lowestNode = ParentFinder(node.left, n1, n2);
                Node right_lowestNode = ParentFinder(node.right, n1, n2);

                if (left_lowestNode != null && right_lowestNode != null)
                {
                    return node;
                }
                return (left_lowestNode != null) ? left_lowestNode : right_lowestNode;

            }
        }


        //Print Tree
        class NodeInfo
        {
            public Node Node;
            public string Text;
            public int StartPos;
            public int Size { get { return Text.Length; } }
            public int EndPos { get { return StartPos + Size; } set { StartPos = value - Size; } }
            public NodeInfo Parent, Left, Right;
        }

        public static void Print(this Node root, string textFormat = "0", int spacing = 1, int topMargin = 2, int leftMargin = 2)
            {
                if (root == null) return;
                int rootTop = Console.CursorTop + topMargin;
                var last = new List<NodeInfo>();
                var next = root;
                for (int level = 0; next != null; level++)
                {
                    var item = new NodeInfo { Node = next, Text = next.data.ToString(textFormat) };
                    if (level < last.Count)
                    {
                        item.StartPos = last[level].EndPos + spacing;
                        last[level] = item;
                    }
                    else
                    {
                        item.StartPos = leftMargin;
                        last.Add(item);
                    }
                    if (level > 0)
                    {
                        item.Parent = last[level - 1];
                        if (next == item.Parent.Node.left)
                        {
                            item.Parent.Left = item;
                            item.EndPos = Math.Max(item.EndPos, item.Parent.StartPos - 1);
                        }
                        else
                        {
                            item.Parent.Right = item;
                            item.StartPos = Math.Max(item.StartPos, item.Parent.EndPos + 1);
                        }
                    }
                    next = next.left ?? next.right;
                    for (; next == null; item = item.Parent)
                    {
                        int top = rootTop + 2 * level;
                        Print(item.Text, top, item.StartPos);
                        if (item.Left != null)
                        {
                            Print("/", top + 1, item.Left.EndPos);
                            Print("_", top, item.Left.EndPos + 1, item.StartPos);
                        }
                        if (item.Right != null)
                        {
                            Print("_", top, item.EndPos, item.Right.StartPos - 1);
                            Print("\\", top + 1, item.Right.StartPos - 1);
                        }
                        if (--level < 0) break;
                        if (item == item.Parent.Left)
                        {
                            item.Parent.StartPos = item.EndPos + 1;
                            next = item.Parent.Node.right;
                        }
                        else
                        {
                            if (item.Parent.Left == null)
                                item.Parent.EndPos = item.StartPos - 1;
                            else
                                item.Parent.StartPos += (item.StartPos - 1 - item.Parent.EndPos) / 2;
                        }
                    }
                }
                Console.SetCursorPosition(0, rootTop + 2 * last.Count - 1);
            }

            private static void Print(string s, int top, int left, int right = -1)
            {
                Console.SetCursorPosition(left, top);
                if (right < 0) right = left + s.Length;
                while (Console.CursorLeft < right) Console.Write(s);
            }
    }
}