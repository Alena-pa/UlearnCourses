using System;

namespace func.brainfuck
{
    public class BrainfuckBasicCommands
    {
        public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
        {
            RegisterIO(vm, read, write);
            RegisterMemoryOperations(vm);
            RegisterPointerShift(vm);
            RegisterAsciiConstants(vm);
        }

        private static void RegisterIO(IVirtualMachine vm, Func<int> read, Action<char> write)
        {
            vm.RegisterCommand('.', m => write((char)m.Memory[m.MemoryPointer]));
            vm.RegisterCommand(',', m => m.Memory[m.MemoryPointer] = (byte)read());
        }

        private static void RegisterMemoryOperations(IVirtualMachine vm)
        {
            vm.RegisterCommand('+', m => m.Memory[m.MemoryPointer] = unchecked((byte)(m.Memory[m.MemoryPointer] + 1)));
            vm.RegisterCommand('-', m => m.Memory[m.MemoryPointer] = unchecked((byte)(m.Memory[m.MemoryPointer] - 1)));
        }


        private static void RegisterPointerShift(IVirtualMachine vm)
        {
            vm.RegisterCommand('>', m =>
            {
                m.MemoryPointer = (m.MemoryPointer + 1) % m.Memory.Length;
            });
            vm.RegisterCommand('<', m =>
            {
                m.MemoryPointer = (m.MemoryPointer - 1 + m.Memory.Length) % m.Memory.Length;
            });
        }

        private static void RegisterAsciiConstants(IVirtualMachine vm)
        {
            foreach (char c in "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789")
                RegisterAsciiCommand(vm, c);
        }

        private static void RegisterAsciiCommand(IVirtualMachine vm, char c)
        {
            byte value = (byte)c;
            vm.RegisterCommand(c, m => m.Memory[m.MemoryPointer] = value);
        }
    }
}
