import {
  Button,
  Card,
  Group,
  Menu,
  Skeleton,
  Stack,
  Text,
  rem,
  useMantineTheme,
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

function GameCard({ id, name, startTime, drawTime, loading }: GameCardProps) {
  const navigate = useNavigate();
  const theme = useMantineTheme();
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
                      color={theme.colors.violet[9]}
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
}

export default GameCard;
