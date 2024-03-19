import { Card, Skeleton, Text } from "@mantine/core";

interface GameCardProps {
  name: string;
  startTime: Date;
  drawTime: Date;
  loading: boolean;
}

function formatDate(date: Date) {
  return date.toLocaleString("en-GB", {
    weekday: "long",
    year: "numeric",
    month: "short",
    day: "numeric",
  });
}

function GameCard({ name, startTime, drawTime, loading }: GameCardProps) {
  return (
    <Card>
      <Skeleton visible={loading} radius={"xl"}>
        <Text fw={500} size="lg" mt="md">
          {name}
        </Text>
      </Skeleton>
      <Skeleton visible={loading} radius={"xl"}>
        <Text>
          {formatDate(startTime)} - {formatDate(drawTime)}
        </Text>
      </Skeleton>
    </Card>
  );
}

export default GameCard;
