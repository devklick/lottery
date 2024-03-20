import {
  ActionIcon,
  Affix,
  Button,
  Card,
  Flex,
  Group,
  List,
  Menu,
  Skeleton,
  Stack,
  Text,
  rem,
} from "@mantine/core";
import { IconBriefcase, IconEdit, IconRotate2 } from "@tabler/icons-react";
import { useNavigate } from "react-router-dom";
import { useUserStore } from "../stores/user.store";

interface GameCardProps {
  id: string;
  name: string;
  startTime: Date;
  drawTime: Date;
  loading: boolean;
}

function formatDate(date: Date) {
  return date.toLocaleString("en-GB", {
    weekday: "short",
    year: "numeric",
    month: "short",
    day: "numeric",
  });
}

function GameCardManageOptions({}: {}) {}

function GameCard({ id, name, startTime, drawTime, loading }: GameCardProps) {
  const navigate = useNavigate();
  const { isUserType } = useUserStore();
  return (
    <Card withBorder shadow="xl" radius="lg">
      <Card.Section withBorder inheritPadding py={"xs"}>
        <Group>
          <Skeleton visible={loading}>
            <Text fw={500}>{name}</Text>
          </Skeleton>
        </Group>
      </Card.Section>

      <Card.Section withBorder inheritPadding py={"xs"}>
        <Stack py={"xs"} gap={"xs"} align="start">
          <Skeleton visible={loading}>
            <Group>
              <Text c="dimmed">Starts on:</Text>
              <Text>{formatDate(startTime)}</Text>
            </Group>
          </Skeleton>
          <Skeleton visible={loading}>
            <Group>
              <Text c="dimmed">Draws on:</Text>
              <Text>{formatDate(drawTime)}</Text>
            </Group>
          </Skeleton>
        </Stack>
      </Card.Section>

      <Card.Section withBorder inheritPadding py={"xs"}>
        <Stack>
          <Skeleton visible={loading}>
            <Group>
              <Button fullWidth onClick={() => navigate(`/games/${id}`)}>
                Play
              </Button>
            </Group>
          </Skeleton>
          {isUserType("Admin") && (
            <Skeleton visible={loading}>
              <Group justify="space-between">
                <Menu withinPortal shadow="sm">
                  <Menu.Target>
                    <Button
                      fullWidth
                      color="red"
                      leftSection={<IconBriefcase size={rem(18)} />}
                    >
                      Manage
                    </Button>
                  </Menu.Target>
                  <Menu.Dropdown>
                    <Menu.Item
                      leftSection={
                        <IconEdit
                          size={rem(18)}
                          onClick={() => navigate(`/games/${id}/edit}`)}
                        />
                      }
                    >
                      Edit
                    </Menu.Item>
                    <Menu.Item leftSection={<IconRotate2 size={rem(18)} />}>
                      Result
                    </Menu.Item>
                  </Menu.Dropdown>
                </Menu>
              </Group>
            </Skeleton>
          )}
        </Stack>
      </Card.Section>
    </Card>
  );

  // return (
  //   <Card withBorder shadow="sm" padding="lg" radius="md" w={"100%"} h={"100%"}>
  //     <IconBurger />
  //     <Skeleton visible={loading} radius={"xl"}>
  //       <Text fw={"bold"} size="xl" mt="md">
  //         {name}
  //       </Text>
  //     </Skeleton>
  //     <Skeleton visible={loading} radius={"xl"}>
  //       <Text>
  //         {formatDate(startTime)} - {formatDate(drawTime)}
  //       </Text>
  //     </Skeleton>
  //     <Flex justify={"flex-end"} align={"flex-end"} h={"100%"} w={"100%"}>
  //       <Skeleton visible={loading}>
  //         <Button
  //           color="green"
  //           mt="md"
  //           radius="md"
  //           onClick={() => navigate(`/games/${id}`)}
  //         >
  //           Play
  //         </Button>
  //       </Skeleton>
  //     </Flex>
  //   </Card>
  // );
}

export default GameCard;
