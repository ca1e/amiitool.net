/*
 * Copyright (C) 2016 Benjamin Krämer
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using LibAmiibo.Helper;

namespace LibAmiibo.Data.Figurine;

public static class AmiiboSet
{
    private static Dictionary<byte, GroupName> dict = new()
    {
        { 0x00, new GroupName("Super Smash Bros.", "SSB") },
        { 0x01, new GroupName("Super Mario Bros.", "SMB") },
        { 0x02, new GroupName("Chibi-Robo!", "CR") },
        { 0x03, new GroupName("Yoshi's Woolly World", "YWW") },
        { 0x04, new GroupName("Splatoon", "Splatoon") },
        { 0x05, new GroupName("Animal Crossing", "AC") },
        { 0x06, new GroupName("8-bit Mario", "8M") },
        { 0x07, new GroupName("Skylanders", "Skylanders") },
        { 0x09, new GroupName("Legend Of Zelda", "LOZ") },
        { 0x0a, new GroupName("Shovel Knight", "SK") },
        { 0x0c, new GroupName("Kirby", "Kirby") },
        { 0x0d, new GroupName("Pokemon", "Pokemon") },
        { 0x0e, new GroupName("Mario Sports Superstars", "MSS") },
        { 0x0f, new GroupName("Monster Hunter", "MH") },
        { 0x10, new GroupName("BoxBoy!", "BB") },
        { 0x11, new GroupName("Pikmin", "Pikmin") },
        { 0x12, new GroupName("Fire Emblem", "FE") },
        { 0x13, new GroupName("Metroid", "Metroid") },
        { 0x14, new GroupName("Others", "Others") },
        { 0x15, new GroupName("Mega Man", "MM") },
        { 0x16, new GroupName("Diablo", "Diablo") },
        { 0x17, new GroupName("Power Pros", "PP") },
        { 0x18, new GroupName("Monster Hunter Rise", "MHR") },
        { 0x19, new GroupName("Yu-Gi-Oh!", "YGO") },
        { 0xFF, new GroupName("(empty)", "?") },
    };

    internal static GroupName GetName(byte id)
    {
        GroupName name;
        if (dict.TryGetValue(id, out name))
            return name;

        return null;
    }
}