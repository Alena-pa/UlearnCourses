using System.Collections.Generic;

namespace func.brainfuck
{
    public class BrainfuckLoopCommands
    {
        public static void RegisterTo(IVirtualMachine vm)
        {
            var loopsByStart = new Dictionary<int, int>();
            var currentLoopsByStart = new Stack<int>();

            vm.RegisterCommand('[', b => HandleLoopStart(b, loopsByStart, currentLoopsByStart));
            vm.RegisterCommand(']', b => HandleLoopEnd(b, currentLoopsByStart));
        }

        private static void HandleLoopStart(
        IVirtualMachine vmState,
        Dictionary<int, int> loopsByStart,
        Stack<int> currentLoopsByStart)
        {
            if (loopsByStart.Count == 0)
                PrecomputeLoops(vmState, loopsByStart);

            if (vmState.Memory[vmState.MemoryPointer] == 0)
                vmState.InstructionPointer = loopsByStart[vmState.InstructionPointer];
            else
                currentLoopsByStart.Push(vmState.InstructionPointer);
        }


        private static void HandleLoopEnd(IVirtualMachine vmState, Stack<int> currentLoopsByStart)
        {
            if (vmState.Memory[vmState.MemoryPointer] != 0)
                vmState.InstructionPointer = currentLoopsByStart.Peek();
            else
                currentLoopsByStart.Pop();
        }

        private static void PrecomputeLoops(IVirtualMachine vmState, Dictionary<int, int> loopsByStart)
        {
            var loopStack = new Stack<int>();

            for (int i = 0; i < vmState.Instructions.Length; i++)
            {
                char instruction = vmState.Instructions[i];
                if (instruction == '[')
                    loopStack.Push(i);
                else if (instruction == ']')
                {
                    int start = loopStack.Pop();
                    loopsByStart[start] = i;
                }
            }
        }
    }
}
