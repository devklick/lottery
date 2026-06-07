import {
  Group,
  MantineColor,
  Overlay,
  Skeleton,
  useMantineTheme,
} from "@mantine/core";
import { IconTrophyFilled, IconTrophyOff } from "@tabler/icons-react";

interface TrophyProps {
  position: number;
  loading: boolean;
  disabled?: boolean;
}

function useTrophyColor(position: number): MantineColor {
  const { colors } = useMantineTheme();
  switch (position) {
    case 1:
      return colors.yellow[6];
    case 2:
      return colors.gray[5];
    case 3:
      return colors.orange[7];
    default:
      return colors.gray[7];
  }
}

function Trophy({ position, disabled, loading }: TrophyProps) {
  const color = useTrophyColor(position);

  const width = 30;
  const iconProps = { size: width };

  const icon = disabled ? (
    <IconTrophyOff {...iconProps} color="gray" />
  ) : (
    <IconTrophyFilled {...iconProps} color={color} />
  );

  const overlay = disabled ? null : (
    <Overlay backgroundOpacity={0} c={"white"}>
      <span style={{ textShadow: "1px 1px 3px black, -1px -1px 3px grey" }}>
        {loading ? "" : position}
      </span>
    </Overlay>
  );

  return (
    <Skeleton visible={loading} w={width}>
      <Group justify="center" style={{ position: "relative" }}>
        {icon}
        {overlay}
      </Group>
    </Skeleton>
  );
}

export default Trophy;
