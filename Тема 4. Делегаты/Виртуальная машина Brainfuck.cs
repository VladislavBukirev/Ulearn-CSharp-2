﻿using System;
using System.Collections.Generic;

namespace func.brainfuck
{
    public class VirtualMachine : IVirtualMachine
    {
        public string Instructions { get; }
        public int InstructionPointer { get; set; }
        public byte[] Memory { get; }
        public int MemoryPointer { get; set; }

        private Dictionary<char, Action<IVirtualMachine>> commandDict = new (); 

        public VirtualMachine(string program, int memorySize)
        {
            Instructions = program;
            Memory = new byte[memorySize];
        }

        public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
        {
            commandDict.Add(symbol, execute);
        }

        public void Run()
        {
            for(; InstructionPointer < Instructions.Length; InstructionPointer++)
            {
                var command = Instructions[InstructionPointer];
                if (commandDict.ContainsKey(command))
                {
                    commandDict[command](this);
                }
            }
        }
    }
}