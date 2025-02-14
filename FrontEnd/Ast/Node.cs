﻿// Copyright (c) Leonardo Brugnara
// Full copyright and license information in LICENSE file

namespace Zenit.Ast
{
    public abstract class Node
    {
        public T Visit<T>(IAstWalker<T> walker) where T : class
        {
            return walker.Visit(this);
        }

        public void Visit(IAstWalker walker)
        {
            walker.Visit(this);
        }

        public virtual string Uid => $"@{this.GetType().Name.Replace("Node", "").ToLower()}{this.GetHashCode()}";
    }
}
