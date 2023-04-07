using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;

namespace func.brainfuck
{
    public class BrainfuckBasicCommands
    {
        public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
        {
            vm.RegisterCommand('.', b => { write((char)vm.Memory[vm.MemoryPointer]); });
            vm.RegisterCommand(',', b => { vm.Memory[vm.MemoryPointer] = (byte)read(); });
            vm.RegisterCommand('+',
                b => { vm.Memory[vm.MemoryPointer] = (byte)((vm.Memory[vm.MemoryPointer] + 1) % 256); });
            vm.RegisterCommand('-', b =>
            {
                var value = vm.Memory[vm.MemoryPointer] - 1;
                if (value >= 0)
                    vm.Memory[vm.MemoryPointer] = (byte)(vm.Memory[vm.MemoryPointer] - 1);
                else
                    vm.Memory[vm.MemoryPointer] = (byte)(vm.Memory[vm.MemoryPointer] + value + 256);
            });
            MovePointer(vm);
            RegisterSymbols(vm);
        }

        private static void RegisterSymbols(IVirtualMachine vm)
        {
            for (var symbol = 'a'; symbol <= 'z'; symbol++)
            {
                var byteSymbol = (byte)symbol;
                vm.RegisterCommand(symbol, b => { vm.Memory[vm.MemoryPointer] = byteSymbol; });
            }

            for (var symbol = 'A'; symbol <= 'Z'; symbol++)
            {
                var byteSymbol = (byte)symbol;
                vm.RegisterCommand(symbol, b => { vm.Memory[vm.MemoryPointer] = byteSymbol; });
            }

            for (var symbol = '0'; symbol <= '9'; symbol++)
            {
                var byteSymbol = (byte)symbol;
                vm.RegisterCommand(symbol, b => { vm.Memory[vm.MemoryPointer] = byteSymbol; });
            }
        }
        
        private static void MovePointer(IVirtualMachine vm)
        {
            vm.RegisterCommand('>', b =>
            {
                if (vm.MemoryPointer < vm.Memory.Length - 1)
                    vm.MemoryPointer++;
                else
                    vm.MemoryPointer = (vm.MemoryPointer + 1) % vm.Memory.Length;
            });
            vm.RegisterCommand('<', b =>
            {
                if (vm.MemoryPointer > 0)
                    vm.MemoryPointer--;
                else
                    vm.MemoryPointer = vm.Memory.Length - 1;
            });
        }
    }
}