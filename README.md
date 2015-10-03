# ntreg-sharp

This library enables a .NET or Mono programmer to read, and to an extent write to, offline NT registry hives.


Write support is basically experimental, and has a few caveats:

1) The new string or data must be less than or equal the size of the original string or data
2) If the string or data is less than the original, null bytes are appended.

I call this lazy writing, because the real way to do this is to update the node and value key data lengths AND the data itself, which requires expanding or contracting the size of the original hive. This is a bit more complicated. For most of my purposes, this lazy-write method has worked well.