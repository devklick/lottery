import { Affix, Button, Card, Flex, Skeleton, Text } from "@mantine/core";
import { useNavigate } from "react-router-dom";

interface GameCardProps {
  id: string;
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

function GameCard({ id, name, startTime, drawTime, loading }: GameCardProps) {
  const navigate = useNavigate();
  return (
    <Card withBorder shadow="sm" padding="lg" radius="md">
      <Skeleton visible={loading} radius={"xl"}>
        <Text fw={"bold"} size="xl" mt="md">
          {name}
        </Text>
      </Skeleton>
      <Skeleton visible={loading} radius={"xl"}>
        <Text>
          {formatDate(startTime)} - {formatDate(drawTime)}
        </Text>
      </Skeleton>
      <Skeleton visible={loading}>
        <Flex justify={"right"}>
          <Button
            color="green"
            mt="md"
            radius="md"
            onClick={() => navigate(`/games/${id}`)}
          >
            Play
          </Button>
        </Flex>
      </Skeleton>
    </Card>
  );
}

export default GameCard;
