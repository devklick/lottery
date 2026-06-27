import {
  MantineColor,
  MantineColorShade,
  useComputedColorScheme,
  useMantineTheme,
} from "@mantine/core";

export function useIsDarkTheme() {
  const colorScheme = useComputedColorScheme();
  return colorScheme === "dark";
}

type ColorOrColorShade = MantineColor | [MantineColor, MantineColorShade];

interface UseColorForThemeParams {
  light: ColorOrColorShade;
  dark: ColorOrColorShade;
}

export function useColorForTheme({
  dark,
  light,
}: UseColorForThemeParams): MantineColor {
  const isDark = useIsDarkTheme();
  const { colors } = useMantineTheme();

  const getColor = (
    color: MantineColor | [MantineColor, MantineColorShade],
  ): MantineColor => {
    if (typeof color === "string") return color;
    const [colorKey, shade] = color;
    return colors[colorKey][shade];
  };

  return isDark ? getColor(dark) : getColor(light);
}

interface UseColorShadeForTheme {
  color: MantineColor;
  light: MantineColorShade;
  dark: MantineColorShade;
}
export function useColorShadeForTheme({
  color,
  dark,
  light,
}: UseColorShadeForTheme) {
  const { colors } = useMantineTheme();

  return useColorForTheme({
    light: [color, light],
    dark: colors[color][dark],
  });
}
