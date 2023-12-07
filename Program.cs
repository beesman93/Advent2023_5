List<string> lines = new List<string>();
using (StreamReader reader = new StreamReader(args[0]))
{
    while (!reader.EndOfStream)
    {
        lines.Add(reader.ReadLine());
    }
}

sln(false);
sln(true);

void sln(bool part2)
{

    List<Tuple<int, int>> rodeos = new List<Tuple<int, int>>();

    int ll = 0;

    bool save_seed = false;

    List<Tuple<ulong, ulong>> seeds = new();

    List<Tuple<ulong, ulong, ulong>> ranges = new List<Tuple<ulong, ulong, ulong>>();

    foreach (var line in lines)
    {

        if (ll == 0)
        {
            List<string> tmp = line.Split(':')[1].Split(' ').ToList<string>();
            for (int i = 1; i < tmp.Count; i++)
            {
                if (part2)
                {
                    if (i % 2 == 0)
                    {
                        seeds.Add(new Tuple<ulong, ulong>(Convert.ToUInt64(tmp[i - 1]), (Convert.ToUInt64(tmp[i - 1]) + Convert.ToUInt64(tmp[i]))));
                    }
                }
                else
                {
                    seeds.Add(new(uint.Parse(tmp[i]), uint.Parse(tmp[i])));
                }
            }
        }

        ll++;
        //Console.WriteLine(ll);

        if (line == "" || ll == lines.Count() - 1)
        {
            //input split - let's map

            List<Tuple<ulong, ulong>> new_seeds = new();

            foreach (var range in ranges)
            {
                ulong rangeFrom = range.Item2;
                ulong rangeTo = range.Item2 + range.Item3;
                for (int i = 0; i < seeds.Count; i++)
                {
                    //split to ranges first
                    if (rangeTo >= seeds[i].Item1 && rangeFrom <= seeds[i].Item2)
                    {
                        ulong newSeedFrom = rangeFrom > seeds[i].Item1 ? rangeFrom : seeds[i].Item1;
                        ulong newSeedTo = rangeTo < seeds[i].Item2 ? rangeTo : seeds[i].Item2;
                        ulong change = range.Item1 - range.Item2;
                        new_seeds.Add(new Tuple<ulong, ulong>(newSeedFrom + change, newSeedTo + change));
                        //cut rest of the seed
                        if (seeds[i].Item1 < newSeedFrom)
                        {
                            ulong newCutBeforeRange_from = seeds[i].Item1;
                            ulong newCutBeforeRange_to = newSeedFrom - 1;
                            seeds.Add(new(newCutBeforeRange_from, newCutBeforeRange_to));
                        }
                        if (seeds[i].Item2 > newSeedTo)
                        {
                            ulong newCutAftrRange_from = newSeedTo + 1;
                            ulong newCutAftrRange_to = seeds[i].Item2;
                            seeds.Add(new(newCutAftrRange_from, newCutAftrRange_to));
                        }
                        seeds[i] = new(ulong.MinValue, ulong.MinValue);
                    }
                }
                //fill the rest
            }
            foreach (var new_seed in new_seeds)
            {
                seeds.Add(new_seed);

                /*foreach (var range in ranges)
                {
                    ulong rangeFrom = range.Item2;
                    ulong rangeTo = range.Item2 + range.Item3;
                    ulong change = range.Item1 - range.Item2;
                    if (change < 0)
                    {
                        //Console.WriteLine(change);
                    }
                    if (new_seed.Item1 >= rangeFrom && new_seed.Item2 <= rangeTo)
                    {
                        seeds.Add(new(new_seed.Item1 + change, new_seed.Item2 + change));
                    }
                }*/
            }


            ranges.Clear();
            continue;
        }

        if (line[0] >= '0' && line[0] <= '9')
        {
            //map instructions
            List<string> lineSplit = line.Split(' ').ToList<string>();
            ulong destinationRangeStart = Convert.ToUInt64(lineSplit[0]);
            ulong sourceRangeStart = Convert.ToUInt64(lineSplit[1]);
            ulong lengthRange = Convert.ToUInt64(lineSplit[2]);
            ranges.Add(new Tuple<ulong, ulong, ulong>(destinationRangeStart, sourceRangeStart, lengthRange - 1));
            continue;
        }
    }

    ulong min = ulong.MaxValue;
    foreach (var seed in seeds)
    {
        if (seed.Item1 != ulong.MinValue && seed.Item1 < min)
            min = seed.Item1;
    }

    Console.WriteLine(min);
}