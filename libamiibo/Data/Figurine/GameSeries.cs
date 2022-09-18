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

public static class GameSeries
{
    private static Dictionary<int, GroupName> dict = new()
    {
        { 0x000,    new GroupName("Super Mario", "SMA") },
        { 0x3FF,    new GroupName("(empty)", "?") },
        // TODO
    };

    internal static GroupName GetName(int id)
    {
        GroupName name;
        if (dict.TryGetValue(id, out name))
            return name;
        // TODO
        if (id >= 0x006 && id <= 0x014)
            return new GroupName("Animal Crossing", "ACR");
        if (id >= 0x064 && id <= 0x075)
            return new GroupName("Pokémon", "POK");

        return null;
    }
}