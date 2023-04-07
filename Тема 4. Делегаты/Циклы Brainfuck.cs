using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;

namespace func.brainfuck
{
    public class BrainfuckLoopCommands
    { 
        public static void RegisterTo(IVirtualMachine vm)
        {
            var openToClose = LoopOperation(vm).Item1;
            var closeToOpen = LoopOperation(vm).Item2;
            vm.RegisterCommand('[', b =>
            {
                if (vm.Memory[vm.MemoryPointer] == 0)
                {
                    vm.InstructionPointer = openToClose[vm.InstructionPointer];
                }
            });
            vm.RegisterCommand(']', b =>
            {
                if (vm.Memory[vm.MemoryPointer] != 0)
                {
                    vm.InstructionPointer = closeToOpen[vm.InstructionPointer];
                }
            });
        }

        private static Tuple<Dictionary<int, int>, Dictionary<int, int>> LoopOperation(IVirtualMachine vm)
        {
            var openToClose = new Dictionary<int, int>();
            var closeToOpen = new Dictionary<int, int>();
            var stack = new Stack<int>();
            for (var i = 0; i < vm.Instructions.Length; i++)
            {
                switch (vm.Instructions[i])
                {
                    case '[':
                    {
                        stack.Push(i);
                        break;
                    }
                    case ']':
                    {
                        var openValue = stack.Pop();
                        openToClose.Add(openValue, i);
                        closeToOpen.Add(i, openValue);
                        break;
                    }
                }
            }
            return new Tuple<Dictionary<int, int>, Dictionary<int, int>>(openToClose, closeToOpen);
        }
    }
}