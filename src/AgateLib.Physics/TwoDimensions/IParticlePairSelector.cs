﻿//
//    Copyright (c) 2006-2018 Erik Ylvisaker
//
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the "Software"), to deal
//    in the Software without restriction, including without limitation the rights
//    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//    copies of the Software, and to permit persons to whom the Software is
//    furnished to do so, subject to the following conditions:
//
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
//
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//    SOFTWARE.
//

using System;
using System.Collections.Generic;

namespace AgateLib.Physics.TwoDimensions
{
    public interface IParticlePairSelector
    {
        IEnumerable<Tuple<PhysicalParticle, PhysicalParticle>> SelectPairs(KinematicsSystem system);
    }

    public class AllPairs : IParticlePairSelector
    {
        public IEnumerable<Tuple<PhysicalParticle, PhysicalParticle>> SelectPairs(KinematicsSystem system)
        {
            for (int i = 0; i < system.Particles.Count; i++)
            {
                for (int j = i + 1; j < system.Particles.Count; j++)
                {
                    yield return new Tuple<PhysicalParticle, PhysicalParticle>(system.Particles[i], system.Particles[j]);
                }
            }
        }
    }
}
