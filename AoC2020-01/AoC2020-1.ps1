$setA = iwr "https://adventofcode.com/2020/day/1/input" | Select-Object -ExpandProperty Content

for ($i = 0; $i -lt $setA.Count; $i++) {
  $a = for ($j = 0; $j -lt $setA.Count; $j++) {
    # Take out loop for part 1
    $b = for ($k = 0; $k -lt $setA.Count; $k++) {
      if ([int]$setA[$i] + [int]$setA[$j] + [int]$setA[$k] -eq 2020) {
        [int]$setA[$i] * [int]$setA[$j] * [int]$setA[$k];
        break;
      }
    }
    if ($b) { $b; break; }
  }
  if ($a) { $a; break; }
}
