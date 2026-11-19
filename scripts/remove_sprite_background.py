"""Remove border-connected neutral mattes from outlined cartoon sprites.

This preserves enclosed white highlights, unlike a global white color key.
Requires Pillow. Example:
    python3 scripts/remove_sprite_background.py source.png transparent.png
"""

import argparse
from collections import deque
from pathlib import Path

from PIL import Image


PROFILES = {
    "white": (220, 255, 35),
    "light-gray": (150, 235, 22),
    "dark-gray": (50, 115, 18),
}


def remove_background(
    source: Path, destination: Path, profile: str = "white"
) -> dict:
    if destination.exists():
        raise FileExistsError(f"Refusing to overwrite {destination}")

    image = Image.open(source).convert("RGBA")
    width, height = image.size
    pixels = list(image.getdata())
    background = bytearray(width * height)
    queue = deque()
    minimum, maximum, neutrality = PROFILES[profile]

    def enqueue(index: int) -> None:
        if background[index]:
            return
        red, green, blue, alpha = pixels[index]
        if alpha == 0 or (
            min(red, green, blue) >= minimum
            and max(red, green, blue) <= maximum
            and max(red, green, blue) - min(red, green, blue) <= neutrality
        ):
            background[index] = 1
            queue.append(index)

    for x in range(width):
        enqueue(x)
        enqueue((height - 1) * width + x)
    for y in range(height):
        enqueue(y * width)
        enqueue(y * width + width - 1)

    while queue:
        index = queue.popleft()
        x, y = index % width, index // width
        if x:
            enqueue(index - 1)
        if x + 1 < width:
            enqueue(index + 1)
        if y:
            enqueue(index - width)
        if y + 1 < height:
            enqueue(index + width)

    output = pixels.copy()
    softened = 0
    for index, (red, green, blue, original_alpha) in enumerate(pixels):
        if background[index]:
            output[index] = (0, 0, 0, 0)
            continue

        x, y = index % width, index // width
        touches_background = (
            (x > 0 and background[index - 1])
            or (x + 1 < width and background[index + 1])
            or (y > 0 and background[index - width])
            or (y + 1 < height and background[index + width])
        )
        lightest = max(red, green, blue)
        darkest = min(red, green, blue)
        if touches_background and lightest - darkest < 65:
            samples = []
            for neighbor_y in range(max(0, y - 1), min(height, y + 2)):
                for neighbor_x in range(max(0, x - 1), min(width, x + 2)):
                    neighbor = neighbor_y * width + neighbor_x
                    if background[neighbor]:
                        samples.append(pixels[neighbor][:3])
            matte = tuple(sum(s[c] for s in samples) / len(samples) for c in range(3))
            outline = (15, 21, 45)
            direction = tuple(outline[c] - matte[c] for c in range(3))
            denominator = sum(d * d for d in direction)
            coverage = (
                sum((value - matte[c]) * direction[c]
                    for c, value in enumerate((red, green, blue)))
                / denominator
            ) if denominator else 1.0
            if 0 < coverage < 1:
                channels = tuple(
                    max(0, min(255, round((channel - matte[c] * (1 - coverage)) / coverage)))
                    for c, channel in enumerate((red, green, blue))
                )
                output[index] = (*channels, round(original_alpha * coverage))
                softened += 1

    result = Image.new("RGBA", image.size)
    result.putdata(output)
    destination.parent.mkdir(parents=True, exist_ok=True)
    result.save(destination)
    return {
        "size": image.size,
        "background_profile": profile,
        "transparent_background_pixels": sum(background),
        "unmatted_edge_pixels": softened,
        "destination": str(destination),
    }


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("source", type=Path)
    parser.add_argument("destination", type=Path)
    parser.add_argument("--background", choices=PROFILES, default="white")
    args = parser.parse_args()
    print(remove_background(args.source, args.destination, args.background))
