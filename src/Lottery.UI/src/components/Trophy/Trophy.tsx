import { Group, MantineColor, Overlay, Skeleton } from "@mantine/core";
import { IconTrophyFilled, IconTrophyOff } from "@tabler/icons-react";

interface TrophyProps {
  position: number;
  loading: boolean;
  disabled?: boolean;
}

const trophyColors = new Map<number, MantineColor>([
  [1, "gold"],
  [2, "silver"],
  [3, "brown"],
]);

function getTrophyColor(position: number) {
  return trophyColors.get(position) ?? "blue";
}

function Trophy({ position, disabled, loading }: TrophyProps) {
  const color = getTrophyColor(position);

  const width = 30;
  const iconProps = { size: width };

  const icon = disabled ? (
    <IconTrophyOff {...iconProps} color="gray" />
  ) : (
    <IconTrophyFilled {...iconProps} color={color} />
  );

  const overlay = disabled ? null : (
    <Overlay backgroundOpacity={0} c={"white"} style={{ lineHeight: 2 }}>
      {loading ? "" : position}
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
