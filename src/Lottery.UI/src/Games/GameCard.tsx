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
    <Card withBorder shadow="sm" padding="lg" radius="md" w={"100%"} h={"100%"}>
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
      <Flex justify={"flex-end"} align={"flex-end"} h={"100%"} w={"100%"}>
        <Skeleton visible={loading}>
          <Button
            color="green"
            mt="md"
            radius="md"
            onClick={() => navigate(`/games/${id}`)}
          >
            Play
          </Button>
        </Skeleton>
      </Flex>
    </Card>
  );
}

export default GameCard;
